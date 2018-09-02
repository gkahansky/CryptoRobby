using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra.MarketData;
using CryptoRobert.Infra;

namespace CryptoRobert.Importer.Base
{
    public interface IDbHandler
    {

        void SaveMarketData(GlobalMarketData mdObject);

        void SaveCoin(string symbol, string name);

        void SaveCoinPairs(List<CoinPair> list);

        Dictionary<CoinInterval, List<TickGap>> FindMissingTicks(Dictionary<string, long> intervals);

        long FindKlineLastUpdate(string symbol, string interval);
    }
}
