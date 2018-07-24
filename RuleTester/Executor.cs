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

namespace RuleTester
{
    public class Executor
    {
        private Dictionary<string, PatternConfig> PatternConfig { get; set; }
        private PatternEngine Engine { get; set; }
        private List<Kline> KlineList { get; set; }


        public void RunTest(ILogger logger, PatternConfig settings, string path)
        {
            KlineList = GenerateKlinesFromCsv(logger, settings, path);
            Engine = InitializeEngine(logger, settings);

            Engine.CheckAllPatterns(KlineList, Engine.Patterns.Items, PatternConfig);
        }

        private PatternEngine InitializeEngine(ILogger logger, PatternConfig settings)
        {
            PatternConfig = new Dictionary<string, PatternConfig>();
            PatternConfiguration(settings);
            var pat = NewPattern(settings.Name, logger, settings);
            PatternEngine engine = new PatternEngine(logger);
            engine.Patterns.Add(pat);

            return engine;
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
            }

            return (Pattern)p;
        }

        private void PatternConfiguration(PatternConfig settings)
        {
            PatternConfig.Add(settings.Name, settings);//PreparePatternSpringConfig(settings));
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
