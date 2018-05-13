using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;

namespace Crypto.Importer.Base
{
    public interface IDbHandler
    {
        IQueryable<User> LoadUsers();

        void SaveMarketData(GlobalMarketData mdObject);

        void SaveCoin(string symbol, string name);

        void SaveCoinPairs(List<CoinPair> list);

    }
}
