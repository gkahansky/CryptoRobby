
using System;
using System.Collections.Generic;
using CryptoRobert.Infra.Patterns;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.RuleEngine.Entities;
using CryptoRobert.RuleEngine.Interfaces;

namespace CryptoRobert.RuleEngine
{
    public interface IDataHandler
    {
        void LoadCoinDataFromDb();

        List<string> LoadCoinDataFromCsv(string path);

        void SavePatterns(Dictionary<string, IPattern> repo);

        List<RuleDefinition> LoadRulesFromDb();
        List<RuleSetDefinition> LoadRuleSetsFromDb();
        RuleDefinition GetRuleById(int Id);

    }
}
