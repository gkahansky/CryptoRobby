using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine.Interfaces
{
    public interface IRuleRepository
    {
        Dictionary<string, Dictionary<string, IRule>> Rules { get; set; }

        void Add(IRule rule);
        void Remove(IRule rule);
        IRule Find(string ruleKey);
        string GenerateKey(IRule rule);
    }
}
