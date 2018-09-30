using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Trading
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

        void BuyPair(Kline kline, StopLossDefinition stopLoss);

        void Sell(Kline kline, out decimal profit);

        bool CheckStopLoss(Kline kline);

    }
}
