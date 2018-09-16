using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.RuleEngine;
using CryptoRobert.RuleEngine.Interfaces;

namespace CryptoRobert.RuleEngine.Entities
{
    public class RulePriceTrend : RuleBase , IRule
    {
        #region Members
        private decimal AvgPrice { get; set; }
        private decimal LastAvgPrice { get; set; }
        private decimal LastPrice { get; set; }

        #endregion
        #region CTOR
        public RulePriceTrend(string symbol, string interval, int retention, int id = 0, decimal value=0)
            : base(symbol, interval, retention, id)
        {
            RuleType = "RulePriceTrend";
        }
        #endregion

        public override void Calculate(Kline kline)
        {
            ProcessKline(kline);
            Value = CalculateTrend();
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
