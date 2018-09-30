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
        decimal MaxProfit { get; set; }
        decimal MinProfit { get; set; }
        DateTime FirstTransactionTime { get; set; }
        DateTime LastTransactionTime { get; set; }

        void BuyPair(Kline kline, IPattern p, string name);

        void Sell(Kline kline, out decimal profit);

        StopLossDefinition GenerateStopLossObject(IPattern p);

        bool CheckStopLoss(IPattern p, Kline kline);

    }
}
