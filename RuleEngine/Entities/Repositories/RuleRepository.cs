using CryptoRobert.Infra;
using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Entities.Repositories
{
    public class RuleRepository : IRuleRepository
    { 
        public Dictionary<string, Dictionary<string,IRule>> Rules { get; set; }
        private ILogger _logger { get; set; }

        #region CTOR
        public RuleRepository(ILogger logger)
        {
            _logger = logger;
            Rules = new Dictionary<string, Dictionary<string, IRule>>();
            
        }

        #endregion

        #region Public Methods
        public void Add(IRule rule, int defId)
        {
            AddRuleToRulesDict(rule);  
        }

        public void Remove(IRule rule)
        {
            if (Rules.ContainsKey(rule.Key))
            {
                var symbolRules = Rules[rule.Symbol];
                if (symbolRules.ContainsKey(rule.Key))
                {
                    symbolRules.Remove(rule.Key);
                }
            }
        }

        public IRule FindByKey(string ruleKey)
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

        public IRule FindById(int id)
        {
            foreach (var ruleBase in this.Rules)
            {
                foreach(var rule in ruleBase.Value)
                {
                    if (rule.Value.Id == id)
                        return rule.Value;
                }
            }
            return null;
        }




        #endregion

        #region Private Methods

        private void AddRuleToRulesDict(IRule rule)
        {
            if (Rules.ContainsKey(rule.Symbol))
            {
                var symbolRules = Rules[rule.Symbol];
                if (!symbolRules.ContainsKey(rule.Key))
                {
                    symbolRules.Add(rule.Key, rule);
                }
            }
            else
            {
                var symbolRules = new Dictionary<string, IRule>();
                symbolRules.Add(rule.Key, rule);
                Rules.Add(rule.Symbol, symbolRules);
            }
        }

        

        #endregion
    }
}
