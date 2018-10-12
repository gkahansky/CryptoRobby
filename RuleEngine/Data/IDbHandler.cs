
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
        List<string> LoadCoinDataFromCsv(string path);

        List<RuleDefinition> LoadRulesFromDb(int id=0);
        List<RuleSet> LoadRuleSetsFromDb(int id=0);
        List<RuleSetDefinition> LoadRuleSetToRulesFromDb();
        RuleDefinition GetRuleById(int Id);
        List<Pair> LoadCoinPairsFromDb();
        bool SaveRuleSet(RuleSet set);
        bool SaveRuleDefinition(RuleDefinition rule);
    }
}
