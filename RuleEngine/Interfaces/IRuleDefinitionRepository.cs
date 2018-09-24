using CryptoRobert.RuleEngine.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Interfaces
{
    public interface IRuleDefinitionRepository
    {
        Dictionary<string,RuleDefinition> Rules { get; set; }

        void Add(RuleDefinition rule);
        RuleDefinition FindByKey(string key);
        RuleDefinition FindById(int id);
        void Remove(string key);
    }
}
