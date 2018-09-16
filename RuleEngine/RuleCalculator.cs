using CryptoRobert.Infra;
using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine
{
    public class RuleCalculator : IRuleCalculator
    {
        public decimal CalculateTrend(decimal LastAvgPrice, decimal avgPrice)
        {
            decimal avgPriceChange = 0;
            if (LastAvgPrice > 0)
            {
                var avgPriceDelta = avgPrice - LastAvgPrice;
                avgPriceChange = (avgPriceDelta / avgPrice);
            }
            return avgPriceChange;
        }
    }
}
