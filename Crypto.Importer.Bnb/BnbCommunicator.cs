using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
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


        string Source { get; set; }
        #endregion



        public BnbCommunicator(ILogger logger, IDbHandler dbHandler) : base(logger, dbHandler)
        {
            _logger = logger;
            _dbHandler = dbHandler;

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
            //LoadUsersFromDatabse();

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
            }
            catch (Exception e)
            {
                _logger.Log("Failed to update coin pairs.\n" + e.ToString());
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

        public List<Kline> GetCandleStickData(string pair, string interval, int limit)
        {
            _logger.Log(string.Format("Retreiving top {2} CandleSticks for {0}, interval = {1}", pair, interval, limit));
            //Load List of pairs & intervals required

            //for each pair-interval combo, find the most recent update

            //Request Data & save to Db for each pair-interval combo

            //request update from last update to now
                var klineRawData = BnbRequestKlinesByInterval(pair, interval, limit);
            //Send data to parser and get list of klines
                var klineList = parser.ConvertKlineStringToList(klineRawData, interval);
            return klineList;
            //Save list of Klines to db

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
            try
            {
                var count = 0;
                foreach (var pair in pairList)
                {
                    if (!CoinPairs.ContainsKey(pair.Symbol))
                    {
                        metaData.CoinPairDict.Add(pair.Symbol, pair);
                        count += 1;
                    }
                }
                return count;
            }
            catch (Exception e)
            {
                _logger.Log("Failed to update coin pair list.\n" + e.ToString());
                return 0;
            }

        }

        //Get Candlestick stats for specific pair and interval
        private string BnbRequestKlinesByInterval(string symbol, string interval, int limit = 1)
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
