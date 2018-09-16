using Crypto.RuleEngine.Interfaces;
using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Entities
{
    public class RuleRepository : IRuleRepository
    { 
        public Dictionary<string, Dictionary<string,IRule>> Rules { get; set; }

        public RuleRepository()
        {
            Rules = new Dictionary<string, Dictionary<string, IRule>>();
        }

        public void Add(IRule rule)
        {
            string ruleKey = GenerateKey(rule);
            if (Rules.ContainsKey(rule.Symbol))
            {
                var symbolRules = Rules[rule.Symbol];
                if (!symbolRules.ContainsKey(ruleKey))
                {
                    symbolRules.Add(ruleKey, rule);
                }   
            }
            else
            {
                var symbolRules = new Dictionary<string, IRule>();
                symbolRules.Add(ruleKey, rule);
                Rules.Add(rule.Symbol, symbolRules);
            }
        }

        public void Remove(IRule rule)
        {
            var ruleKey = GenerateKey(rule);
            if (Rules.ContainsKey(rule.Symbol))
            {
                var symbolRules = Rules[rule.Symbol];
                if (symbolRules.ContainsKey(ruleKey))
                {
                    symbolRules.Remove(ruleKey);
                }
            }
        }

        public IRule Find(string ruleKey)
        {
            var symbol = ruleKey.Substring(0, ruleKey.IndexOf('_'));
            if (Rules.ContainsKey(symbol))
            {
                if (Rules[symbol].ContainsKey(ruleKey))
                    return Rules[symbol][ruleKey];
                else
                    return null;
            }
            else
                return null;
        }

        public string GenerateKey(IRule rule)
        {
            string[] attributes = { rule.Symbol, rule.Interval, rule.Retention.ToString(), rule.RuleType };
            string key = string.Join("_", attributes);
            return key;
        }
    }
}
