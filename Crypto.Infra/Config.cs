﻿using System;
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
        public static bool BnbGetHistoricalData { get; set; }
        public static bool BnbFillGapsMode { get; set; }
        public static string CmcExchange { get; set; }
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
        public static int LogSeverity { get; internal set; }
        public static long BnbMinimumUpdateDate { get; set; }
        public static bool UseSql { get; set; }
        public static bool RecordTicksToFile { get; set; }
        public static string[] BnbPairs { get; set; }
        public static List<string> intervalsToMonitor { get; set; }
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
            UseSql = bool.Parse(json["UseSql"].ToString());
            SetLogSeverity(json);

            //Populate Configuration params
            _logger.Info("********Loading CMC Configuration********");
            GetCmcConfiguration(json, _logger);
            _logger.Info("********Loading BNB Configuration********");
            GetBnbConfiguration(json, _logger);
            _logger.Info("********Loading SQL Configuration********");
            BuildSqlConnectionString(json, _logger);
            _logger.Info("********Loading RabbitMQ Configuration********");
            LoadRabbitMQConfiguration(json, _logger);
            _logger.Info("********Loading DBhandler Configuration********");
            LoadDbHandlerConfiguration(json, _logger);
            _logger.Info("********Loading Patterns Configuration********");
            LoadPatternsConfiguration(json, _logger);

            PairsOfInterest = new List<CoinPair>();
            PatternSpringThreshold = 0.03m;
            PatternSpringToKeep = 20;
        }



        private static void SetLogSeverity(JObject json)
        {
            var sev = json["LogSeverity"].ToString().ToLower();
            switch (sev)
            {
                case "debug":
                    LogSeverity = 0;
                    break;
                case "info":
                    LogSeverity = 1;
                    break;
                case "warning":
                    LogSeverity = 2;
                    break;
                case "error":
                    LogSeverity = 3;
                    break;
                default:
                    LogSeverity = 1;
                    break;
            }
        }

        public static void ReloadConfiguration()
        {
            Parser parser = new Parser(_logger);
            var configString = File.ReadAllText(Path);
            var json = parser.ParseTextToJson(configString);
            _logger.Info("********RefreshinG Patterns & Interesting Pair Configuration********");
            LoadPatternsConfiguration(json, _logger);
        }

        private static void GetBnbConfiguration(JObject json, ILogger _logger)
        {
            var bnbJson = json["BnbConfiguration"];

            BinanceApiKey = bnbJson["BinanceApiKey"].ToString();
            BinanceApiSecret = bnbJson["BinanceApiSecret"].ToString();
            BinanceSampleInterval = int.Parse(bnbJson["SampleInterval"].ToString());
            BnbExchange = bnbJson["RabbitExchange"].ToString();
            BnbGetHistoricalData = bool.Parse(bnbJson["BnbGetHistoricalData"].ToString());
            RecordTicksToFile = bool.Parse(bnbJson["RecordTicksToFile"].ToString());
            BnbFillGapsMode = bool.Parse(bnbJson["FillGapsMode"].ToString());
            BnbPairs = GetBnbPairs(bnbJson);
            BnbMinimumUpdateDate = ConvertTimeStringToMs(bnbJson["BnbMinimumUpdateDate"].ToString());

            GetIntervalsToMonitor(json);


            _logger.Info(String.Format("Binance API Key: {0}", BinanceApiKey));
            _logger.Info(String.Format("Binance API Secret: {0}", BinanceApiSecret));
            _logger.Info(String.Format("Binance Sample Interval: {0}", BinanceSampleInterval));
            _logger.Info(String.Format("BnbImporter Exchange: {0}", BnbExchange));
            _logger.Info(String.Format("BnbImporter Get History Mode: {0}", BnbGetHistoricalData));
            _logger.Info(String.Format("BnbImporter Minimum Time For Historical Data: {0}", BnbMinimumUpdateDate));
        }

        private static string[] GetBnbPairs(JToken bnbJson)
        {
            var pairsString = bnbJson["Pairs"].ToString();
            var pairs = pairsString.Split(',');
            return pairs;
        }

        private static void GetIntervalsToMonitor(JObject json)
        {
            intervalsToMonitor = new List<string>();
            var intervals = json["PairsToMonitor"]["Intervals"].ToString().Split(',');
            foreach(var interval in intervals)
            {
                var start = interval.IndexOf('"')+1;
                var end = interval.LastIndexOf('"');
                var intervalClean = interval.Substring(start, end-start);
                intervalsToMonitor.Add(intervalClean);
                
                _logger.Info("Interval Added: " + intervalClean);
            }
        }

        private static long ConvertTimeStringToMs(string dateTimeString)
        {
            long timeLong = Parser.ConvertTimeDateTimeToMs(DateTime.Now);
            try
            {
                var time = DateTime.Parse(dateTimeString);
                timeLong = Parser.ConvertTimeDateTimeToMs(time);
                return timeLong;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to parse Minimum Time for historical data. proceeding with live data.\n" + e.ToString());
                return timeLong;
            }

        }

        private static void GetCmcConfiguration(JObject json, ILogger _logger)
        {
            var cmcJson = json["CmcConfiguration"];
            CmcSampleInterval = int.Parse(cmcJson["SampleInterval"].ToString());
            CmcExchange = cmcJson["RabbitExchange"].ToString();
            _logger.Info(String.Format("CMC Sample Interval: {0}", CmcSampleInterval));
            _logger.Info(String.Format("CMC Exchange: {0}", CmcExchange));
        }

        private static void BuildSqlConnectionString(JObject json, ILogger _logger)
        {
            try
            {
                SqlConnectionString = json["SqlConnectionString"].ToString();
                _logger.Info(String.Format("SQL Connection String: {0}", SqlConnectionString));
            }
            catch (Exception e)
            {
                _logger.Info("Failed to generate SQL Connection string.\n" + e.ToString());
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
                _logger.Info(String.Format("RabbitMQ host : {0}, User : {1}, Pass : {2}", RabbitHost, RabbitUser, RabbitPass));
                foreach (var e in RabbitExchanges)
                {
                    _logger.Info("New Exchange: " + e);
                }
            }
            catch (Exception e)
            {
                _logger.Info("Failed to load RabbitMQ Configuration.\n" + e.ToString());
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
                _logger.Info(String.Format("DbHandler Queue : {0}", DbHandlerQueue));
            }
            catch (Exception e)
            {
                _logger.Info("Failed to load RabbitMQ Configuration.\n" + e.ToString());
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
