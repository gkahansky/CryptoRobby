using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Crypto.Infra;
using Crypto.RuleEngine;
using Crypto.RuleEngine.Patterns;
using Newtonsoft.Json.Linq;

namespace RuleTester
{
    public class Executor
    {
        private Dictionary<string, JObject> PatternConfig { get; set; }

        public void Execute(ILogger logger, JObject settings)
        {
            PatternConfig = new Dictionary<string, JObject>();
            PatternConfiguration(settings);

            var pFactory = new PatternFactory(logger);
            var data = new DataHandler(logger);

            var coinData = data.LoadCoinDataFromCsv(settings["Path"].ToString());

            Parser parser = new Parser(logger);
            var klineList = parser.ParseKlinesFromCsvToList(coinData, settings["Symbol"].ToString(), settings["Interval"].ToString());

            var pat = new SimpleSpringPattern(logger, PatternConfig["Spring"]);
            PatternEngine pEngine = new PatternEngine(logger);
            pEngine.Patterns.Add(pat);

            pEngine.CalculatePatternsFromDataFeed(klineList, pEngine.Patterns);
        }

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
            Config.PatternSpringThreshold = decimal.Parse(settings["Threshold"].ToString());
            Config.PatternSpringToKeep = int.Parse(settings["Retention"].ToString());
            return springConfig;
        }
    }
}
