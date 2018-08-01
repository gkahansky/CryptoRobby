using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CryptoRobert.Infra;
using CryptoRobert.RuleEngine;
using CryptoRobert.Infra.Patterns;
using CryptoRobert.RuleEngine.Transactions;
using Newtonsoft.Json.Linq;
using CryptoRobert.RuleEngine.Patterns;

namespace RuleTester
{
    public class Executor
    {
        private Dictionary<string, PatternConfig> PatternConfig { get; set; }
        private PatternEngine Engine { get; set; }
        private List<Kline> KlineList { get; set; }
        private ILogger logger { get; set; }

        public Executor(ILogger _logger)
        {
            logger = _logger;
        }
        //New
        public delegate void KlineReceivedEventHandler(object source, EventArgs args);
        public event KlineReceivedEventHandler KlineReceived;


        public void RunTest(ILogger logger, PatternConfig settings, string path)
        {
            KlineList = GenerateKlinesFromCsv(logger, settings, path);
            //Legacy
            //Engine = InitializeEngine(logger, settings);
            //Engine.CheckAllPatterns(KlineList, Engine.Patterns.Items, PatternConfig);

            //New
            DataRepository dataRepository = new DataRepository();

            InitializePatternConfiguration(settings);
            
            var runner = new PatternRunner(logger, dataRepository);

            PublishKlines(KlineList, runner);

        }



        private void PublishKlines(List<Kline> klineList, PatternRunner runner)
        {
            foreach(var kline in klineList)
            {
                runner.RunPatterns(kline);
            }
            logger.Info("TOTAL PROFIT OF ALL TRADES: " + runner.TradeResults.Sum() + "%");
        }


        private void InitializePatternConfiguration(PatternConfig settings)
        {
            PatternConfig = new Dictionary<string, PatternConfig>();
            PatternConfiguration(settings);
            var pat = NewPattern(settings.Name, logger, settings);
            Config.PatternsConfig = new Dictionary<string, PatternConfig>();
            var hash = pat.Name + "_" + pat.Symbol + "_" + pat.Interval;
            Config.PatternsConfig.Add(hash, settings);
        }

        protected virtual void OnKlineReceived()
        {
            if (KlineReceived != null)
            {
                KlineReceived(this, EventArgs.Empty);
            }
        }


        private List<Kline> GenerateKlinesFromCsv(ILogger logger, PatternConfig settings, string path)
        {
            var data = new DataHandler(logger);
            var coinData = data.LoadCoinDataFromCsv(path);

            Parser parser = new Parser(logger);
            var klineList = parser.ParseKlinesFromCsvToList(coinData, settings.Symbol, settings.Interval);

            return klineList;
        }

       
        private Pattern NewPattern(string name, ILogger logger, PatternConfig settings)
        {
            var p = new Object();

            switch (name)
            {
                case "Spring":
                    {
                        p = new SpringPattern(logger, settings);
                        break;
                    }
                case "Streak":
                    {
                        p = new StreakPattern(logger, settings);
                        break;
                    }
                case "TrendShift":
                    {
                        p = new TrendShiftPattern(logger, settings);
                        break;
                    }
                case "TrendIncline":
                    {
                        p = new TrendInclinePattern(logger, settings);
                        break;
                    }
                    
            }

            return (Pattern)p;
        }

        private void PatternConfiguration(PatternConfig settings)
        {
            PatternConfig.Add(settings.Name, settings);//PreparePatternSpringConfig(settings));
        }

        private void PreparePatternSpringConfig(JObject settings)
        {
            //Legacy
            //var springConfig = new JObject();
            //springConfig.Add("Name", settings["Name"]);
            //springConfig.Add("Symbol", settings["Symbol"]);
            //springConfig.Add("Interval", settings["Interval"]);
            //springConfig.Add("Threshold", settings["Threshold"]);
            //springConfig.Add("Retention", settings["Retention"]);
            //springConfig.Add("DefaultSLThreshold", settings["DefaultSLThreshold"]);
            //springConfig.Add("DynamicSLThreshold", settings["DynamicSLThreshold"]);
            //Config.PatternSpringThreshold = decimal.Parse(settings["Threshold"].ToString());
            //Config.PatternSpringToKeep = int.Parse(settings["Retention"].ToString());

            //New
            Config.PatternsConfig = new Dictionary<string, PatternConfig>();
            var pConfig = new PatternConfig();
            pConfig.Name = settings["Name"].ToString();
            pConfig.Symbol= settings["Symbol"].ToString();
            pConfig.Interval = settings["Interval"].ToString();
            pConfig.Threshold= decimal.Parse(settings["Threshold"].ToString());
            pConfig.Retention= int.Parse(settings["Retention"].ToString());
            pConfig.DefaultStopLoss= decimal.Parse(settings["DefaultSLThreshold"].ToString());
            pConfig.DynamicStopLoss= decimal.Parse(settings["DynamicSLThreshold"].ToString());
            pConfig.IsActive = true;
            Config.PatternSpringThreshold = decimal.Parse(settings["Threshold"].ToString());
            Config.PatternSpringToKeep = int.Parse(settings["Retention"].ToString());
            var hash = pConfig.Name + "_" + pConfig.Symbol + "_" + pConfig.Interval;
            Config.PatternsConfig.Add(hash, pConfig);
        }
    }
}
