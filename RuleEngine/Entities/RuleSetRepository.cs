using Crypto.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine.Entities
{
    public class RuleSetRepository : IRuleSetRepository
    {
        public Dictionary<int, RuleSet> RuleSets { get; set; }

        public RuleSetRepository()
        {
            RuleSets = new Dictionary<int, RuleSet>();
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

        public RuleSet Find(int id, int ruleId)
        {
            if (RuleSets.Count() > 0)
            {
                var set = RuleSets[id];
                return set;
            }
            else
                return null;
        }
    }
}
