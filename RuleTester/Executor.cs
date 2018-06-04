using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Crypto.Infra;
using Crypto.RuleEngine;
using Newtonsoft.Json.Linq;

namespace RuleTester
{
    public class Executor
    {
        private Dictionary<string, JObject> PatternConfig { get; set; }

        public void Execute(ILogger logger, JObject settings)
        {
            PatternConfig = new Dictionary<string, JObject>();
            PatternConfiguration();

            var pFactory = new PatternFactory(logger);
            var data = new DataHandler(logger);

            var coinData = data.LoadCoinDataFromCsv(settings["Path"].ToString());

            Parser parser = new Parser(logger);
            var klineList = parser.ParseKlinesFromCsvToList(coinData, settings["Symbol"].ToString(), settings["Interval"].ToString());

            var pat = new PatternSpring(logger, PatternConfig["Spring"]);
            PatternEngine pEngine = new PatternEngine(logger);
            pEngine.Patterns.Add(pat);

            pEngine.CalculatePatternsFromDataFeed(klineList, pEngine.Patterns);
        }

        private void PatternConfiguration()
        {
            PatternConfig.Add("Spring", PreparePatternSpringConfig());
        }

        private JObject PreparePatternSpringConfig()
        {
            var springConfig = new JObject();
            springConfig.Add("Symbol", "");
            springConfig.Add("Interval", "");
            springConfig.Add("Threshold", Config.PatternSpringThreshold);
            springConfig.Add("Retention", Config.PatternSpringToKeep);
            return springConfig;
        }
    }
}
