using CryptoRobert.RuleEngine.Entities;
using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Interfaces
{
    public interface IRuleSetRepository
    {
        Dictionary<int, RuleSet> RuleSets { get; set; }

        void Add(RuleSet ruleSet);
        void Remove(int id);
        RuleSet Find(int id);
        RuleSet FindSetConfiguration(int id, int ruleId);
    }
}
