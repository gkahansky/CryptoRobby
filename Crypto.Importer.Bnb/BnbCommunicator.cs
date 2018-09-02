using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using M3C.Finance.BinanceSdk;
using M3C.Finance.BinanceSdk.Methods;
using M3C.Finance.BinanceSdk.ResponseObjects;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Rabbit;
using CryptoRobert.Importer.Base;
using System.Configuration;
using RabbitMQ.Client;
using Crypto.Infra.MarketData;

namespace CryptoRobert.Importer.Bnb
{
    public class BnbCommunicator : Base.Importer
    {
        #region Members
        public BinanceClient restClient { get; set; }
        public Dictionary<string, CoinPair> CoinPairs { get; set; }
        public Parser parser { get; set; }
        public MetaData metaData { get; set; }
        private readonly ILogger _logger;
        private readonly IDbHandler _dbHandler;
        private readonly IRabbitHandler _rabbit;
        private readonly IFileHandler _fileHandler;
        private bool StartupComplete { get; set; }


        //string Source { get; set; }
        #endregion

        public BnbCommunicator(ILogger logger, IDbHandler dbHandler, IRabbitHandler rabbit, IFileHandler fileHandler) : base(logger, dbHandler)
        {
            _logger = logger;
            _dbHandler = dbHandler;
            _rabbit = rabbit;
            _fileHandler = fileHandler;
            StartupComplete = false;

            _logger.Info("    Starting Binance Importer...");
            _logger.Info("=====================================");
            _logger.Info("Binance Importer Started Successfully");
            _logger.Info("=====================================\n");

            _logger.Info("Loading configuration...");
            Config.LoadConfiguration(_logger);
            parser = new Parser(logger);
            _logger.Info("Parser Initialized Successfully");
            metaData = new MetaData();
            _logger.Info("Metadata Lists Initialized Successfully");
            CoinPairs = new Dictionary<string, CoinPair>();
            _rabbit.Connect();
        }

        public void FillGapsinDb()
        {
            _logger.Info("Working in 'Fill-Gaps' Mode. Looking for gaps in database...");
            var gaps = _dbHandler.FindMissingTicks(this.metaData.Intervals);
            GetKlinesForDbGaps(gaps);
        }

        private void GetKlinesForDbGaps(Dictionary<CoinInterval, List<TickGap>> gaps)
        {
            foreach (var item in gaps)
            {
                foreach (var gap in item.Value)
                {
                    var klineList = DownloadKlinesFromBnb(item.Key.Symbol, item.Key.Interval, gap.From, gap.To);
                    var klines = klineList.Result;
                    if (klines.Count > 0)
                        saveKlines(klines);
                }
            }
        }

        #region Connectivity


        public BinanceClient Connect()
        {
            restClient = InitRestClient(Config.BinanceApiKey, Config.BinanceApiSecret);
            var time = restClient.TimeSync();
            _logger.Info("Binance Connection Successfull. Server Time: " + time.ServerTime.ToString());
            return restClient;
        }

        private BinanceClient InitRestClient(string apiKey, string secretKey)
        {
            //You need to init client with Api Keys for Signed Endpoint requests
            var restClient = new BinanceClient(apiKey, secretKey);
            //Logger.Log("Binance Client initiated succesfully");
            return restClient;
        }
        #endregion

        #region Metadata Public
        public void UpdateTickerPrices()
        {
            try
            {
                //var restClient = Connect();
                //_logger.Info("Working in No DB Mode - Fetching Coin Pairs from BNB...");
                var tickers = new List<CoinPair>();

                //if (Config.BnbUseSql)
                //    tickers = UpdateTickerPricesToDb();
                //else
                tickers = UpdateTickerPricesToDictionary();

                var newTickers = SavePairsToMetadata(tickers);
                _logger.Info(newTickers + " new Coin Pairs were loaded");

                if (!StartupComplete)
                {
                    BnbSelectPairsOfInterest(tickers);
                    _logger.Info("Number of \"Pairs of interest\" found: " + Config.PairsOfInterest.Count);
                }
            }
            catch (Exception e)
            {
                _logger.Info("Failed to update coin pairs.\n" + e.ToString());
            }




        }

        private List<CoinPair> UpdateTickerPricesToDictionary()
        {
            var tickers = new List<CoinPair>();
            try
            {

                //var restClient = Connect();
                _logger.Info("Working in No DB Mode - Fetching Coin Pairs from BNB...");
                tickers = GetCoinPairsFromBinance();
                return tickers;
            }
            catch (Exception e)
            {
                _logger.Info("Failed to update coin pairs.\n" + e.ToString());
                return tickers;
            }
        }

        public async Task SaveCandleStickData()
        {
            _logger.Info("Retreiving latest CandleStick data");
            //Load List of pairs & intervals required

            List<string> intervals = Config.intervalsToMonitor;
            //new List<string> { /*"1m", "3m",*/ "5m", "15m", "30m", "1h",/* "2h","4h", "6h", "8h", "12h", "1d", "3d", "1w", "1M"*/ };



            //for each pair-interval combo, find the most recent update
            foreach (var pair in Config.PairsOfInterest)
            {
                _logger.Info("Getting Candlestick data for " + pair.Symbol);
                await GetKlines(pair, Config.intervalsToMonitor);
            }
        }

        //Get Last Close for kline of symbol-interval combo
        public async Task<long> LoadKlineLastUpdate(string symbol, string interval)
        {
            try
            {
                var last = _dbHandler.FindKlineLastUpdate(symbol, interval);
                return last;
            }
            catch (Exception e)
            {
                _logger.Info("Failed to load last update date for kline.\n" + e.ToString());
                return -1;
            }
        }

        public List<CoinPair> GetCoinPairsFromBinance()
        {
            var tickersString = BnbRequestAllTickers().Result;
            var pairList = parser.ParseBnbTickers(tickersString);
            return pairList;
        }

        #endregion

        #region Metada Private
        //Load all coinpairs from Bnb
        private async Task GetKlines(CoinPair pair, List<string> intervals)
        {
            foreach (var interval in intervals)
            {
                try
                {
                    var now = Calculator.CalculateEpochNow();
                    //Get last update from db for this pair-kline combo

                    var lastTask = CheckPairLastUpdate(pair, interval, now);
                    var last = lastTask.Result;

                    var IsUpdateRequired = Calculator.CheckIfUpdateRequired(now, last, metaData.Intervals[interval]);
                    if (IsUpdateRequired)
                    {
                        var klineListTask = DownloadKlinesFromBnb(pair.Symbol, interval, last, now);
                        var klineList = klineListTask.Result;
                        //Save klines to KlinesQueue.
                        var maxClose = GetMaxCloseTimeFromList(klineList);
                        saveKlines(klineList);
                        pair.LastUpdate[interval] = maxClose;
                        _logger.Info(String.Format("Kline data for {0} {1} saved successfully", pair.Symbol, interval));
                    }
                    else
                        await _logger.LogAsync(String.Format("{0} {1} is up to date, moving to next interval", pair.Symbol, interval));

                }
                catch (Exception e)
                {
                    await _logger.LogAsync(String.Format("Failed to get kline data.\n{0}", e.ToString()), 4);
                }
            }
        }

        private void saveKlines(List<Kline> klineList)
        {
            if (Config.RecordTicksToFile)
                _fileHandler.SaveKlineToFile(klineList);
            else
            {
                MetaDataContainer.KlineQueue.Enqueue(klineList);
                _rabbit.PublishKlineList(klineList);
            }
        }

        private async Task<List<Kline>> DownloadKlinesFromBnb(string symbol, string interval, long timeFrom, long timeTo)
        {
            //Download Klines from BNB
            var klineRawData = await BnbRequestKlinesByTime(symbol, interval, timeFrom, timeTo);
            //Convert raw data to list of Klines
            var klineList = parser.ConvertKlineStringToList(klineRawData, interval, symbol);

            return klineList;
        }

        private long GetMaxCloseTimeFromList(List<Kline> klineList)
        {
            long maxClose = klineList[0].CloseTime;

            foreach (var kline in klineList)
            {
                if (kline.CloseTime > maxClose)
                    maxClose = kline.CloseTime;
            }
            return maxClose;
        }

        private async Task<long> CheckPairLastUpdate(CoinPair pair, string interval, long now)
        {
            long last = 0;
            if (Config.UseSql)
                last = _dbHandler.FindKlineLastUpdate(pair.Symbol, interval);
            else
                last = Config.BnbMinimumUpdateDate;

            var diff = metaData.Intervals[interval] + 1000;
            //If not get historical data
            if (!Config.BnbGetHistoricalData)
            {
                if (!pair.LastUpdate.ContainsKey(interval))
                    pair.LastUpdate.Add(interval, now - diff);
            }
            //When we want historical data
            else
            {
                if (pair.LastUpdate.ContainsKey(interval))
                {
                    if (Config.UseSql)
                        pair.LastUpdate[interval] = await LoadKlineLastUpdate(pair.Symbol, interval);
                    else
                        pair.LastUpdate[interval] = last;
                    if (last > Config.BnbMinimumUpdateDate)
                        pair.LastUpdate[interval] = last;
                    else
                        pair.LastUpdate[interval] = Config.BnbMinimumUpdateDate;
                }
                else
                {
                    pair.LastUpdate.Add(interval, Config.BnbMinimumUpdateDate);
                    if (last > Config.BnbMinimumUpdateDate)
                        pair.LastUpdate[interval] = last;
                }
            }
            _logger.Info(string.Format("{0} {1} last update: {2}", pair.Symbol, interval, pair.LastUpdate[interval]));
            return pair.LastUpdate[interval];
        }

        private List<CoinPair> GetAllTickers(BinanceClient publicRestClient)
        {
            //Get All Ticker Information
            var tickers = publicRestClient.TickerAllPrices().Result;
            var pairList = new List<CoinPair>();
            foreach (var ticker in tickers)
            {
                var pair = new CoinPair();
                pair.Symbol = ticker.Symbol;
                pair.Value = ticker.Price;
                pairList.Add(pair);
            }
            return pairList;
        }

        //Save CoinPairs to Metada
        private int SavePairsToMetadata(List<CoinPair> pairList)
        {
            var count = 0;
            try
            {
                foreach (var pair in pairList)
                {
                    if (!metaData.CoinPairDict.ContainsKey(pair.Symbol))
                    {
                        metaData.CoinPairDict.Add(pair.Symbol, pair);
                        count += 1;
                    }
                }
                return count;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to update coin pair list.\n" + pairList[count].Symbol + e.ToString());
                return 0;
            }

        }

        //Get Candlestick stats for specific pair and interval
        private string BnbRequestKlinesByLimit(string symbol, string interval, int limit = 1)
        {

            string url = String.Format(@"https://www.binance.com/api/v1/klines?symbol={0}&interval={1}&limit={2}",
                symbol, interval, limit);
            //Logger.Log("Requesting klines... \nUrl: " + url);
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            var responseFromServer = reader.ReadToEnd();
            // Display the content.

            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();


            return responseFromServer;
        }

        //Download all Tickers from Binance
        private async Task<string> BnbRequestAllTickers()
        {
            string url = String.Format(@"https://www.binance.com/api/v3/ticker/price");
            //Logger.Log("Requesting klines... \nUrl: " + url);
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            var responseAsync = await request.GetResponseAsync();
            HttpWebResponse response = (HttpWebResponse)responseAsync;
            // Display the status.
            Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            var responseFromServer = reader.ReadToEnd();
            // Display the content.

            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();


            return responseFromServer;
        }

        //Download Candlestick Data from BNB by time frame
        private async Task<string> BnbRequestKlinesByTime(string symbol, string interval, long startTime, long endTime)
        {

            if (startTime <= 0)
                //startTime = 1514764800000; //2018-01-01 00:00:00.000
                startTime = 1496275200000; //2017-06-01 00:00:00.000

            string url = String.Format(@"https://www.binance.com/api/v1/klines?symbol={0}&interval={1}&startTime={2}&endTime={3}",
                symbol, interval, startTime, endTime);
            //Logger.Log("Requesting klines... \nUrl: " + url);
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            var responseAsync = await request.GetResponseAsync();
            HttpWebResponse response = (HttpWebResponse)responseAsync;
            // Display the status.
            Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            var responseFromServer = reader.ReadToEnd();
            // Display the content.

            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();


            return responseFromServer;
        }

        public void BnbSelectPairsOfInterest(List<CoinPair> tickers)
        {
            try
            {
                //foreach (var pair in tickers)
                //{
                //    if (Config.PairsToMonitor.ContainsKey(pair.Symbol))
                //        Config.PairsOfInterest.Add(pair);
                //}
                //if (Config.PairsToMonitor.ContainsKey("ETHBTC"))
                //    Config.PairsOfInterest.Add(new CoinPair { Symbol = "ETHBTC" });

                foreach(var symbol in Config.PairsToMonitor)
                {
                    var pair = new CoinPair { Symbol = symbol.Key };
                    Config.PairsOfInterest.Add(pair);
                }


                StartupComplete = true;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to update Pairs of interest, importer startup failed.\n" + e.ToString());
                throw;
            }
        }


        #endregion

        #region Trading

        #endregion


        public void Leave()
        {
            _logger.Info("=================================");
            _logger.Info("   Binance Importer Stopped");
            _logger.Info("=================================");
        }
    }
}
