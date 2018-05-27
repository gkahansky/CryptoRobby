using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Crypto.Infra
{
    public static class Config
    {
        #region Members
        public static string BinanceApiKey { get; set; }
        public static string BinanceApiSecret { get; set; }
        public static int BinanceSampleInterval { get; set; }
        public static List<CoinPair> PairsOfInterest { get; set; }
        public static int CmcSampleInterval { get; set; }
        public static string SqlConnectionString { get; set; }
        #endregion

        public static void LoadConfiguration(ILogger _logger)
        {
            Parser parser = new Parser(_logger);
            //Get Configuration File Path
            var path = @"C:\Crypto\Configuration.json";

            //Download config file and convert to json
            var configString = File.ReadAllText(path);
            var json = parser.ParseTextToJson(configString);

            //Populate Configuration params
            GetCmcConfiguration(json, _logger);
            GetBnbConfiguration(json, _logger);
            BuildSqlConnectionString(json, _logger);
            PairsOfInterest = new List<CoinPair>();
        }

        private static void GetBnbConfiguration(JObject json, ILogger _logger)
        {
            var bnbJson = json["BnbConfiguration"];

            BinanceApiKey = bnbJson["BinanceApiKey"].ToString();
            BinanceApiSecret= bnbJson["BinanceApiSecret"].ToString();
            BinanceSampleInterval = int.Parse(bnbJson["SampleInterval"].ToString());

            _logger.Log(String.Format("Binance API Key: {0}", BinanceApiKey));
            _logger.Log(String.Format("Binance API Secret: {0}", BinanceApiSecret));
            _logger.Log(String.Format("Binance Sample Interval: {0}", BinanceSampleInterval));
        }

        private static void GetCmcConfiguration(JObject json, ILogger _logger)
        {
            var cmcJson = json["CmcConfiguration"];
            CmcSampleInterval = int.Parse(cmcJson["SampleInterval"].ToString());
            _logger.Log(String.Format("CMC Sample Interval: {0}", CmcSampleInterval));
        }
        private static void BuildSqlConnectionString(JObject json, ILogger _logger)
        {
            try
            {
                SqlConnectionString = json["SqlConnectionString"].ToString();
                _logger.Log(String.Format("SQL Connection String: {0}", SqlConnectionString));
            }
            catch (Exception e)
            {
                _logger.Log("Failed to generate SQL Connection string.\n" + e.ToString());
                throw;
            }

        }

    }
}
