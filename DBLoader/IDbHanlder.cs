using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;

namespace CryptoRobert.DBLoader
{
    public interface IDbHandler
    {
        IQueryable<User> LoadUsers();

        void SaveMarketData(GlobalMarketData mdObject);

        void SaveCoin(string symbol, string name);

        void SaveCoinPairs(List<CoinPair> list);

        void SaveKlines(List<Kline> list);

        long FindKlineLastUpdate(string symbol, string interval);
    }
}
