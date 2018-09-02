using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra.Rules
{
    public class RulePriceTrendUp : Rule
    {
        public RulePriceTrendUp(string symbol, string interval, int retention, decimal threshold, int priority, IAction action = null)
            : base(symbol, interval, retention, threshold, priority, action)
        {

        }

        public override bool CheckRule(Kline kline)
        {
            IsAchieved = false;
            ProcessKline(kline);

            var trend = CalculateTrend();

            if (trend > this.Threshold)
                IsAchieved = true;
            return IsAchieved;
        }

        private decimal CalculateTrend()
        {
            decimal avgPriceChange = 0;
            if (LastAvgPrice > 0)
            {
                var avgPriceDelta = AvgPrice - LastAvgPrice;
                avgPriceChange = (avgPriceDelta / AvgPrice);
            }
            return avgPriceChange;
        }
    }
}
