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
        public static string BnbExchange { get; set; }
        public static string CmcExchange { get; set; }
        public static List<CoinPair> PairsOfInterest { get; set; }
        public static int CmcSampleInterval { get; set; }
        public static string SqlConnectionString { get; set; }
        public static string RabbitHost { get; set; }
        public static string RabbitUser { get; set; }
        public static string RabbitPass { get; set; }
        public static string DbHandlerQueue { get; set; }
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
            _logger.Log("********Loading CMC Configuration********");
            GetCmcConfiguration(json, _logger);
            _logger.Log("********Loading BNB Configuration********");
            GetBnbConfiguration(json, _logger);
            _logger.Log("********Loading SQL Configuration********");
            BuildSqlConnectionString(json, _logger);
            _logger.Log("********Loading RabbitMQ Configuration********");
            LoadRabbitMQConfiguration(json, _logger);
            _logger.Log("********Loading DBhandler Configuration********");
            LoadDbHandlerConfiguration(json, _logger);
            PairsOfInterest = new List<CoinPair>();
        }

        private static void GetBnbConfiguration(JObject json, ILogger _logger)
        {
            var bnbJson = json["BnbConfiguration"];

            BinanceApiKey = bnbJson["BinanceApiKey"].ToString();
            BinanceApiSecret= bnbJson["BinanceApiSecret"].ToString();
            BinanceSampleInterval = int.Parse(bnbJson["SampleInterval"].ToString());
            BnbExchange = bnbJson["RabbitExchange"].ToString();

            _logger.Log(String.Format("Binance API Key: {0}", BinanceApiKey));
            _logger.Log(String.Format("Binance API Secret: {0}", BinanceApiSecret));
            _logger.Log(String.Format("Binance Sample Interval: {0}", BinanceSampleInterval));
            _logger.Log(String.Format("BnbImporter Exchange: {0}", BnbExchange));
        }

        private static void GetCmcConfiguration(JObject json, ILogger _logger)
        {
            var cmcJson = json["CmcConfiguration"];
            CmcSampleInterval = int.Parse(cmcJson["SampleInterval"].ToString());
            CmcExchange = cmcJson["RabbitExchange"].ToString();
            _logger.Log(String.Format("CMC Sample Interval: {0}", CmcSampleInterval));
            _logger.Log(String.Format("CMC Exchange: {0}", CmcExchange));
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

        private static void LoadRabbitMQConfiguration(JObject json, ILogger _logger)
        {
            try
            {
                var rabbitConf = json["RabbitMQConfiguration"];
                RabbitHost = rabbitConf["Host"].ToString();
                RabbitUser = rabbitConf["UserName"].ToString();
                RabbitPass = rabbitConf["Password"].ToString();
                _logger.Log(String.Format("RabbitMQ host : {0}, User : {1}, Pass : {2}", RabbitHost, RabbitUser, RabbitPass));
            }
            catch (Exception e)
            {
                _logger.Log("Failed to load RabbitMQ Configuration.\n" + e.ToString());
                throw;
            }
        }

        private static void LoadDbHandlerConfiguration(JObject json, ILogger _logger)
        {
            try
            {
                var DbConf = json["DbHandlerConfiguration"];
                DbHandlerQueue = DbConf["QueueName"].ToString();
                _logger.Log(String.Format("DbHandler Queue : {0}", DbHandlerQueue));
            }
            catch (Exception e)
            {
                _logger.Log("Failed to load RabbitMQ Configuration.\n" + e.ToString());
                throw;
            }
        }
    }
}
