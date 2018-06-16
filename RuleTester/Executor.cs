using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var name = settings["Name"].ToString();

            PatternConfig = new Dictionary<string, JObject>();
            PatternConfiguration(settings);

            var pat = NewPattern(name, logger, PatternConfig[name]);
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

       
        private Pattern NewPattern(string name, ILogger logger, JObject settings)
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
            }

            return (Pattern)p;
        }

        private void PatternConfiguration(JObject settings)
        {
            PatternConfig.Add(settings["Name"].ToString(), PreparePatternSpringConfig(settings));
        }

        private JObject PreparePatternSpringConfig(JObject settings)
        {
            var springConfig = new JObject();
            springConfig.Add("Name", settings["Name"]);
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
