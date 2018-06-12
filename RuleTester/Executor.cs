using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Crypto.Infra;
using Crypto.RuleEngine;
using Crypto.RuleEngine.Patterns;
using Crypto.RuleEngine.Transactions;
using Newtonsoft.Json.Linq;

namespace RuleTester
{
    public class Executor
    {
        private Dictionary<string, JObject> PatternConfig { get; set; }
        private PatternEngine Engine { get; set; }
        private List<Kline> KlineList { get; set; }


        public void RunTest(ILogger logger, JObject settings)
        {
            KlineList = GenerateKlinesFromCsv(logger, settings);
            Engine = InitializeEngine(logger, settings);

            Engine.CheckAllPatterns(KlineList, Engine.Patterns.Items, PatternConfig);
        }

        private PatternEngine InitializeEngine(ILogger logger, JObject settings)
        {
            PatternConfig = new Dictionary<string, JObject>();
            PatternConfiguration(settings);

            var pat = new SimpleSpringPattern(logger, PatternConfig["Spring"]);
            //pat.DefaultStopLossThreshold = decimal.Parse(PatternConfig["DefaultSLThreshold"].ToString());
            PatternEngine engine = new PatternEngine(logger);
            engine.Patterns.Add(pat);


            return engine;
        }

        private List<Kline> GenerateKlinesFromCsv(ILogger logger, JObject settings)
        {
            var data = new DataHandler(logger);
            var coinData = data.LoadCoinDataFromCsv(settings["Path"].ToString());

            Parser parser = new Parser(logger);
            var klineList = parser.ParseKlinesFromCsvToList(coinData, settings["Symbol"].ToString(), settings["Interval"].ToString());

            return klineList;
        }

        //public void Execute(ILogger logger, JObject settings)
        //{
        //    PatternConfig = new Dictionary<string, JObject>();
        //    PatternConfiguration(settings);

        //    //var factory = new PatternFactory(logger);
            
        //    var pat = new SimpleSpringPattern(logger, PatternConfig["Spring"]);
        //    PatternEngine engine = new PatternEngine(logger);
        //    engine.Patterns.Add(pat);

        //    engine.CheckAllPatterns(klineList, engine.Patterns, patternsConfig);
            

        //    //engine.CalculatePatternsFromDataFeed(KlineList, engine.Patterns);
        //}



        private void PatternConfiguration(JObject settings)
        {
            PatternConfig.Add("Spring", PreparePatternSpringConfig(settings));
        }

        private JObject PreparePatternSpringConfig(JObject settings)
        {
            var springConfig = new JObject();
            springConfig.Add("Symbol", settings["Symbol"]);
            springConfig.Add("Interval", settings["Interval"]);
            springConfig.Add("Threshold", settings["Threshold"]);
            springConfig.Add("Retention", settings["Retention"]);
            springConfig.Add("DefaultSLThreshold", settings["DefaultSLThreshold"]);
            springConfig.Add("DynamicSLThreshold", settings["DynamicSLThreshold"]);
            Config.PatternSpringThreshold = decimal.Parse(settings["Threshold"].ToString());
            Config.PatternSpringToKeep = int.Parse(settings["Retention"].ToString());
            return springConfig;
        }
    }
}
