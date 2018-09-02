using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra.MarketData
{
    public class CoinInterval
    {
        public string Symbol { get; set; }
        public string Interval { get; set; }

        public CoinInterval(string symbol, string interval)
        {
            Symbol = symbol;
            Interval = interval;
        }
    }
}
