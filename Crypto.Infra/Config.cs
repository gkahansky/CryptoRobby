using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CryptoRobert.Infra
{
    public static class Config
    {
        #region Members
        public static string Path { get; set; }
        public static string BinanceApiKey { get; set; }
        public static string BinanceApiSecret { get; set; }
        public static int BinanceSampleInterval { get; set; }
        public static string BnbExchange { get; set; }
        public static bool BnbUseSql { get; set; }
        public static string CmcExchange { get; set; }
        public static bool CmcUseSql { get; set; }
        public static List<CoinPair> PairsOfInterest { get; set; }
        public static int CmcSampleInterval { get; set; }
        public static string SqlConnectionString { get; set; }
        public static string RabbitHost { get; set; }
        public static string RabbitUser { get; set; }
        public static string RabbitPass { get; set; }
        public static string DbHandlerQueue { get; set; }
        public static Dictionary<string, PatternConfig> PatternsConfig { get; set; }
        public static decimal PatternSpringThreshold { get; set; }
        public static int PatternSpringToKeep { get; set; }
        public static string[] RabbitExchanges { get; set; }
        public static Dictionary<string, string> PairsToMonitor { get; set; }
        private static ILogger _logger;
        public static bool TestMode { get; set; }
        #endregion

        public static void LoadConfiguration(ILogger logger, bool testMode = false)
        {
            _logger = logger;
            TestMode = testMode;
            Parser parser = new Parser(_logger);
            //Get Configuration File Path
            Path = @"C:\Crypto\Configuration.json";

            //Download config file and convert to json
            var configString = File.ReadAllText(Path);
            var json = parser.ParseTextToJson(configString);
            PairsToMonitor = new Dictionary<string, string>();

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
            _logger.Log("********Loading Patterns Configuration********");
            LoadPatternsConfiguration(json, _logger);

            PairsOfInterest = new List<CoinPair>();
            PatternSpringThreshold = 0.03m;
            PatternSpringToKeep = 20;
        }

        public static void ReloadConfiguration()
        {
            Parser parser = new Parser(_logger);
            var configString = File.ReadAllText(Path);
            var json = parser.ParseTextToJson(configString);
            _logger.Log("********Refreshin Patterns & Interesting Pair Configuration********");
            LoadPatternsConfiguration(json, _logger);
        }
        private static void GetBnbConfiguration(JObject json, ILogger _logger)
        {
            var bnbJson = json["BnbConfiguration"];

            BinanceApiKey = bnbJson["BinanceApiKey"].ToString();
            BinanceApiSecret = bnbJson["BinanceApiSecret"].ToString();
            BinanceSampleInterval = int.Parse(bnbJson["SampleInterval"].ToString());
            BnbExchange = bnbJson["RabbitExchange"].ToString();
            BnbUseSql = bool.Parse(bnbJson["UseSql"].ToString());

            _logger.Log(String.Format("Binance API Key: {0}", BinanceApiKey));
            _logger.Log(String.Format("Binance API Secret: {0}", BinanceApiSecret));
            _logger.Log(String.Format("Binance Sample Interval: {0}", BinanceSampleInterval));
            _logger.Log(String.Format("BnbImporter Exchange: {0}", BnbExchange));
            _logger.Log(String.Format("BnbImporter Use Sql: {0}", BnbUseSql));
        }

        private static void GetCmcConfiguration(JObject json, ILogger _logger)
        {
            var cmcJson = json["CmcConfiguration"];
            CmcSampleInterval = int.Parse(cmcJson["SampleInterval"].ToString());
            CmcExchange = cmcJson["RabbitExchange"].ToString();
            CmcUseSql = bool.Parse(cmcJson["UseSql"].ToString());
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
                RabbitExchanges = ExtractRabbitExchanges(rabbitConf);
                _logger.Log(String.Format("RabbitMQ host : {0}, User : {1}, Pass : {2}", RabbitHost, RabbitUser, RabbitPass));
                foreach (var e in RabbitExchanges)
                {
                    _logger.Log("New Exchange: " + e);
                }
            }
            catch (Exception e)
            {
                _logger.Log("Failed to load RabbitMQ Configuration.\n" + e.ToString());
                throw;
            }
        }

        private static string[] ExtractRabbitExchanges(JToken rabbitConf)
        {
            var exchanges = rabbitConf["Exchanges"].ToObject<string[]>();
            return exchanges;
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

        private static void LoadPatternsConfiguration(JObject json, ILogger _logger)
        {
            PatternsConfig = new Dictionary<string, PatternConfig>();

            var patterns = json["PatternsConfiguration"];
            var pairs = json["PairsToMonitor"]["Pairs"];
            var intervals = json["PairsToMonitor"]["Intervals"];

            foreach (var token in patterns)
            {
                var isActive = token["IsActive"].ToString();
                if(isActive.ToLower() == "true")
                {
                    foreach (var pair in pairs)
                    {
                        foreach (var interval in intervals)
                        {
                            var p = new PatternConfig();
                            p = token.ToObject<PatternConfig>();
                            p.Symbol = pair.ToString();
                            p.Interval = interval.ToString();
                            var hash = p.Name + "_" + p.Symbol + "_" + p.Interval;
                            PatternsConfig.Add(hash, p);
                            AddPairsToMonitor(p);
                        }
                    }
                }
                
            }
        }

        private static void AddPairsToMonitor(PatternConfig p)
        {
            if (!PairsToMonitor.ContainsKey(p.Symbol))
                PairsToMonitor.Add(p.Symbol, p.Interval);
        }
    }
}
