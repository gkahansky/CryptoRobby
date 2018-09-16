using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Patterns;
using RuleTester.Entities;

namespace RuleTester
{
    public class SettingsBuilder : ISettingsBuilder
    {
        public Dictionary<string, PatternConfig> SettingsRepository { get; set; }
        private ILogger logger;

        #region CTOR
        public SettingsBuilder(ILogger _logger)
        {
            logger = _logger;
        }
        #endregion

        public Dictionary<string, IPattern> GenerateSettings(TesterOutput output)
        {
            var patternsRepository = new Dictionary<string, IPattern>();
            var patterns = GetRelevantPatterns(output.Patterns);

            foreach (var p in patterns)
            {
                foreach (var symbol in output.Symbols)
                {
                    foreach (var interval in output.Intervals)
                    {
                        for (var r = output.retention.Min; r <= output.retention.Max; r += output.retention.Increment)
                        {
                            for (var t = output.threshold.Min; t <= output.threshold.Max; t += output.threshold.Increment)
                            {
                                for (var def = output.defaultSLThreshold.Min; def <= output.defaultSLThreshold.Max; def += output.defaultSLThreshold.Increment)
                                {
                                    for (var dyn = output.dynamicSLThreshold.Min; dyn <= output.dynamicSLThreshold.Max; dyn += output.dynamicSLThreshold.Increment)
                                    {
                                        var patternConfig = new PatternConfig(p, symbol, interval, true, t, (int)r, def, dyn);
                                        var hash = p + "_" + symbol + "_" + interval + "_"+ r + "_" + t + "_" + def + "_" + dyn;
                                        var pattern = NewPattern(p, patternConfig, hash);
                                        patternsRepository.Add(hash, pattern);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return patternsRepository;
        }

        private Pattern NewPattern(string name, PatternConfig settings, string engineName)
        {
            var p = new Object();

            switch (name)
            {
                case "Spring":
                    {
                        p = new SpringPattern(logger, settings, engineName);
                        break;
                    }
                case "Streak":
                    {
                        p = new StreakPattern(logger, settings, engineName);
                        break;
                    }
                case "TrendShift":
                    {
                        p = new TrendShiftPattern(logger, settings, engineName);
                        break;
                    }
                case "TrendIncline":
                    {
                        p = new TrendInclinePattern(logger, settings, engineName);
                        break;
                    }
            }

            return (Pattern)p;
        }

        private List<string> GetRelevantPatterns(Dictionary<string, bool> patterns)
        {
            var list = new List<string>();

            foreach (var p in patterns)
            {
                if (p.Value)
                    list.Add(p.Key);
            }
            return list;
        }
    }
}
