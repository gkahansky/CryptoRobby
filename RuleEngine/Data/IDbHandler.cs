
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.RuleEngine.Entities;
using CryptoRobert.RuleEngine.Entities.MetaData;

namespace CryptoRobert.RuleEngine
{
    public interface IDataHandler
    {
        void LoadCoinDataFromDb();

        List<string> LoadCoinDataFromCsv(string path);

        List<RuleDefinition> LoadRulesFromDb();
        List<RuleSet> LoadRuleSetsFromDb();
        List<RuleSetDefinition> LoadRuleSetToRulesFromDb();
        RuleDefinition GetRuleById(int Id);

    }
}
