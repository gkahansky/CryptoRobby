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
    public class RuleDefinitionRepository : IRuleDefinitionRepository
    {
        public Dictionary<string, RuleDefinition> Rules { get; set; }
        private ILogger _logger { get; set; }

        public RuleDefinitionRepository(ILogger logger)
        {
            Rules = new Dictionary<string, RuleDefinition>();
            _logger = logger;
        }

        public void Add(RuleDefinition ruleDef)
        {
            var key = ruleDef.Key;
            if (Rules.Count > 0)
            {
                if (!Rules.ContainsKey(key))
                {
                    Rules.Add(key, ruleDef);
                }
            }
            else
            {
                Rules.Add(key, ruleDef);
            }
        }

        public RuleDefinition FindByKey(string key)
        {
            if (Rules.Count() > 0)
            {
                if (Rules.ContainsKey(key))
                    return Rules[key];
                else
                    return null;
            }
            else
                return null;
        }

        public RuleDefinition FindById(int id)
        {
            if (Rules.Count() > 0)
            {
                foreach (var rule in Rules)
                {
                    if (rule.Value.Id == id)
                        return rule.Value;
                }
                return null;
            }
            else
                return null;
        }

        public void Remove(string key)
        {
            if (Rules.Count > 0)
            {
                if (!Rules.ContainsKey(key))
                {
                    Rules.Remove(key);
                }
            }
        }
    }
}
