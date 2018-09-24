using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Interfaces
{
    public interface IRuleRepository
    {
        Dictionary<string, Dictionary<string, IRule>> Rules { get; set; }

        void Add(IRule rule, int defId);
        void Remove(IRule rule);
        IRule FindByKey(string ruleKey);
        IRule FindById(int id);
    }
}
