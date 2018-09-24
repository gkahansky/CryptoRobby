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
    public class RuleSetRepository : IRuleSetRepository
    {
        public Dictionary<int, RuleSet> RuleSets { get; set; }
        private ILogger _logger { get; set; }

        public RuleSetRepository(ILogger logger)
        {
            RuleSets = new Dictionary<int, RuleSet>();
            _logger = logger;
        }

        public void Add(RuleSet set)
        {
            if (!RuleSets.ContainsKey(set.Id))
            {
                RuleSets.Add(set.Id, set);
            }
        }

        public void Remove(int id)
        {
            if (RuleSets.ContainsKey(id))
            {
                RuleSets.Remove(id);
            }
        }

        public RuleSet Find(int id)
        {
            if (RuleSets.Count() > 0)
            {
                if (RuleSets.ContainsKey(id))
                    return RuleSets[id];
                else
                    return null;
            }
            else
                return null;
        }

        public RuleSet FindSetConfiguration(int id, int ruleId)
        {
            if (RuleSets.Count() > 0)
            {
                if (RuleSets.ContainsKey(id))
                {
                    var set = RuleSets[id];
                    return set;
                }
                else
                    return null;
            }
            else
                return null;
        }

        
    }
}
