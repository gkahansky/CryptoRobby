using CryptoRobert.Infra;
using CryptoRobert.Infra.Patterns;
using CryptoRobert.Infra.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra.Trading
{
    public interface ITradeEngine
    {
        Dictionary<string, StopLossDefinition> StopLossCollection { get; }
        Dictionary<string, Transaction> Transactions { get; }
        string Name { get; set; }
        List<decimal> TradeResults { get; set; }

        void BuyPair(Kline kline, IPattern p, string name);

        void Sell(string symbol, decimal price, out decimal profit);

        StopLossDefinition GenerateStopLossObject(IPattern p);

        bool CheckStopLoss(IPattern p, Kline kline);

    }
}
