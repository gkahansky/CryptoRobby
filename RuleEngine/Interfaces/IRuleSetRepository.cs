using Crypto.RuleEngine.Entities;
using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine.Interfaces
{
    public interface IRuleSetRepository
    {
        void Add(RuleSet ruleSet);
        void Remove(int id);
        RuleSet Find(int id, int ruleId);
    }
}
