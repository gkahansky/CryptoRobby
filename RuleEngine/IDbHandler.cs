
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine
{
    public interface IDataHandler
    {
        void LoadCoinDataFromDb();

        List<string> LoadCoinDataFromCsv(string path);
    }
}
