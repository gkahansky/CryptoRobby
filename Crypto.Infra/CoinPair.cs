using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra
{
    public class CoinPair 

    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Value { get; set; }
        public Dictionary<string, Queue<decimal>> LastPrices { get; set; }
        public decimal AvgPrice { get; set; }
        public long AvgPriceOpenTime { get; set; }
    }

    
}
