using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using CryptoRobert.Importer.Base;
using CryptoRobert.Infra;

namespace CryptoRobert.Importer.Cmc
{
    public class CmcCommunicator : Base.Importer
    {
        private Parser parser { get; set; }
        private readonly ILogger _logger;
        private readonly IDbHandler _dbHandler;

        public CmcCommunicator(ILogger logger, IDbHandler dbHandler) : base(logger, dbHandler)
        {
            _logger = logger;
            _dbHandler = dbHandler;
            _logger.Info("    Starting CMC Importer...");
            _logger.Info("=================================");
            _logger.Info("CMC Importer Started Successfully");
            _logger.Info("=================================\n");

            _logger.Info("Loading configuration...");
            Config.LoadConfiguration(_logger);
            parser = new Parser(logger);
        }

        #region Public Methods
        public void DownloadTickersFromCmc(int maxRank)
        {
            try
            {
                //Get All coins from CMC to list of string
                var tickerStringList = GetCmcTickersInChunks(maxRank);
                //Parse string to list of JObject 
                var coins = parser.ParseCmcCoins(tickerStringList);
                //Save Objects to Db
                SaveAllCoinsToDb(coins);
            }
            catch (Exception e)
            {
                _logger.Error("Failed acquiring tickers from CoinMarketCap.\n" + e.ToString());
            }
        }

        public void CmcRequestGlobalMarketCap()
        {
            try
            {
                _logger.Info("Requesting Global MarketCap Data");
                string url = @"https://api.coinmarketcap.com/v1/global/";
                var mdString = RequestTickers(url);
                var mdObject = parser.ParseCmcMarketData(mdString);
                _dbHandler.SaveMarketData(mdObject);
                _logger.Info(String.Format("Global Market Data Updated. Current Value in USD: {0}$.", mdObject.MarketDataUsd));
            }
            catch (Exception e)
            {
                _logger.Error("Failed to download Global Market Data statistics from CMC.\n" + e.ToString());
            }
        }

        public void UpdateCmcData()
        {
            DownloadTickersFromCmc(500);
            CmcRequestGlobalMarketCap();
        }


        #endregion

        #region Private Methods
        private  void SaveAllCoinsToDb(List<JObject> coins)
        {
            foreach (var coin in coins)
            {
                _dbHandler.SaveCoin(coin["symbol"].ToString(), coin["name"].ToString());
            }
        }

        private List<string> GetCmcTickersInChunks(int maxRank)
        {
            try
            {
                int start = 0;
                int limit = 100;
                int gap = maxRank - start;
                List<string> tickers = new List<string>();

                while (start < maxRank)
                {
                    if (gap >= limit)
                    {
                        _logger.Info(String.Format("Requesting CMC tickers range {0}-{1}", start, limit));
                        var CmcResponse = RequestTickers("https://api.coinmarketcap.com/v1/ticker/?start=" + start + "&limit" + limit);
                        start += limit;
                        gap = maxRank - start;
                        tickers.Add(CmcResponse);
                    }
                    else
                    {
                        limit = gap;
                        _logger.Info(String.Format("Requesting CMC tickers range {0}-{1}", start, limit));
                        var CmcResponse = RequestTickers("https://api.coinmarketcap.com/v1/ticker/?start=" + start + "&limit" + limit);
                        start += limit;
                        tickers.Add(CmcResponse);
                    }
                }

                return tickers;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to download tickers from CoinMarketCap.\n" + e.ToString());
                throw;
            }
        }

        private string RequestTickers(string url)
        {
            try
            {
                string html = string.Empty;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                return html;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to receive rates from CoinMarketCap.\n" + e.ToString());
                return null;
            }
        }

        private string CmcRequestTickerInfo(string url)
        {
            try
            {
                string htmlResponse = string.Empty;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    htmlResponse = reader.ReadToEnd();
                }

                return htmlResponse;
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                throw;
            }

        }


        #endregion

        public void Leave()
        {
            _logger.Info("=================================");
            _logger.Info("     CMC Importer Stopped");
            _logger.Info("=================================");
        }
    }
}
