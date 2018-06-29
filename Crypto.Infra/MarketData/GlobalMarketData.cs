using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Infra
{
    public class GlobalMarketData
    {
        public int Id { get; set; }
        public decimal MarketDataUsd { get; set; }
        public decimal Volume24Hours { get; set; }
        public decimal BitcoinDominancePct { get; set; }
        public int ActiveCurrencies { get; set; }
        public int ActiveAssets { get; set; }
        public int ActiveMarkets { get; set; }
        //public DateTime LastUpdate { get; set; }
    }
}
