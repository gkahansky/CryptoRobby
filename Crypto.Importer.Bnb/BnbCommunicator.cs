using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using M3C.Finance.BinanceSdk;
using M3C.Finance.BinanceSdk.Methods;
using M3C.Finance.BinanceSdk.ResponseObjects;
using Crypto.Infra;
using Crypto.Importer.Base;

namespace Crypto.Importer.Bnb
{
    public class BnbCommunicator : Base.Importer
    {
        #region Members
        public BinanceClient restClient { get; set; }
        public Dictionary<string, CoinPair> CoinPairs { get; set; }
        public Parser parser { get; set; }
        public MetaData metaData { get; set; }
        private readonly ILogger _logger;
        readonly IDbHandler _dbHandler;
        private bool StartupComplete { get; set; }
        //public Queue<List<Kline>> KlineQueue { get; set; }

        //string Source { get; set; }
        #endregion
        

        public BnbCommunicator(ILogger logger, IDbHandler dbHandler) : base(logger, dbHandler)
        {
            _logger = logger;
            _dbHandler = dbHandler;
            StartupComplete = false;

            _logger.Log("    Starting Binance Importer...");
            _logger.Log("=====================================");
            _logger.Log("Binance Importer Started Successfully");
            _logger.Log("=====================================\n");

            _logger.Log("Loading configuration...");
            Config.LoadConfiguration(_logger);
            parser = new Parser(logger);
            _logger.Log("Parser Initialized Successfully");
            metaData = new MetaData();
            _logger.Log("Metadata Lists Initialized Successfully");
            CoinPairs = new Dictionary<string, CoinPair>();
            //KlineQueue = new Queue<List<Kline>>();
        }



        #region Connectivity
        public BinanceClient Connect()
        {
            restClient = InitRestClient(Config.BinanceApiKey, Config.BinanceApiSecret);
            var time = restClient.TimeSync();
            _logger.Log("Binance Connection Successfull. Server Time: " + time.ServerTime.ToString());
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

        #region Metadata
        public void UpdateTickerPrices()
        {
            try
            {
                var restClient = Connect();
                _logger.Log("Fetching Coin Pairs from BNB...");
                var tickers = GetAllTickers(restClient);
                _dbHandler.SaveCoinPairs(tickers);
                var newTickers = SavePairsToMetadata(tickers);
                _logger.Log(newTickers + " new Coin Pairs were loaded");

                if (!StartupComplete)
                {
                    BnbSelectPairsOfInterest(tickers);
                    _logger.Log("Number of \"Pairs of interest\" found: " + Config.PairsOfInterest.Count);
                }


            }
            catch (Exception e)
            {
                _logger.Log("Failed to update coin pairs.\n" + e.ToString());
            }
        }

        public async Task SaveCandleStickData()
        {
            _logger.Log("Retreiving latest CandleStick data");
            string klineRawData = null;
            //Load List of pairs & intervals required
            List<string> intervals = new List<string> { /*"1m", "3m",*/ "5m", "15m", "30m", "1h",/* "2h",*/ "4h",/* "6h", "8h", "12h",*/ "1d"/*, "3d", "1w", "1M"*/ };


            //for each pair-interval combo, find the most recent update
            foreach (var pair in Config.PairsOfInterest)
            {
                foreach (var interval in intervals)
                {
                    try
                    {
                        //Get last update from db for this pair-kline combo
                        var last = LoadKlineLastUpdate(pair.Symbol, interval);
                        _logger.Log(String.Format("Pair: {0}, Interval: {1}, LastUpdate: {2}", pair.Symbol, interval, last));
                        //Request Data & save to Db for each pair-interval combo
                        var now = Calculator.CalculateEpochNow();
                        _logger.Log("Current Time (Epoch): " + now);
                        var klineRawDataTask = BnbRequestKlinesByTime(pair.Symbol, interval, last, now);
                        klineRawData = await klineRawDataTask;
                        //Convert raw data to list of Klines
                        var klineList = parser.ConvertKlineStringToList(klineRawData, interval, pair.Symbol);
                        //Save klines to Db.
                        //KlineQueue.Enqueue(klineList);
                        _dbHandler.SaveKlines(klineList);
                    }
                    catch (Exception e)
                    {
                        _logger.Log(String.Format("Failed to get kline data.\n{0}", e.ToString()));
                    }
                }
            }
        }

        public void LoadUsersFromDatabse()
        {
            var users = _dbHandler.LoadUsers();
            foreach (var u in users)
            {
                metaData.UserDict.Add(u.Id, u);
            }
        }

        //Get Last Close for kline of symbol-interval combo
        public long LoadKlineLastUpdate(string symbol, string interval)
        {
            try
            {
                var last = _dbHandler.FindKlineLastUpdate(symbol, interval);
                return last;
            }
            catch (Exception e)
            {
                _logger.Log("Failed to load last update date for kline.\n" + e.ToString());
                return -1;
            }
        }
        
        private BinanceClient InitClientByUserId(int id)
        {
            try
            {
                if (metaData.UserDict.ContainsKey(id))
                {
                    var user = metaData.UserDict[id];
                    var restClient = InitRestClient(user.BinanceAPI, user.BinanceSecret);
                    return restClient;
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.Log("Failed to initialize restclient for user");
                return null;
            }
        }

        //Load all coinpairs from Bnb
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
                _logger.Log("Failed to update coin pair list.\n" + pairList[count].Symbol + e.ToString());
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

        private async Task<string> BnbRequestKlinesByTime(string symbol, string interval, long startTime, long endTime)
        {

            if (startTime <= 0)
                startTime = 1514764800000; //2018-01-01 00:00:00.000
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

        private void BnbSelectPairsOfInterest(List<CoinPair> tickers)
        {
            try
            {
                foreach (var pair in tickers)
                {
                    if (pair.Symbol.Substring(pair.Symbol.Length - 3) == "BTC")
                        Config.PairsOfInterest.Add(pair);
                }
                StartupComplete = true;
            }
            catch (Exception e)
            {
                _logger.Log("Failed to update Pairs of interest, importer startup failed.\n" + e.ToString());
                throw;
            }
        }


        #endregion

        #region Trading

        #endregion

        public void Leave()
        {
            _logger.Log("=================================");
            _logger.Log("   Binance Importer Stopped");
            _logger.Log("=================================");
        }
    }
}
