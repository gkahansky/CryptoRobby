using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Infra
{
    public class CoinPair 

    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Value { get; set; }
        public Dictionary<string, Queue<decimal>> LastPrices { get; set; }
        public decimal AvgPrice { get; set; }
        public long AvgPriceOpenTime { get; set; }
        public Dictionary<string, long> LastUpdate { get; set; }

        public CoinPair()
        {
            LastPrices = new Dictionary<string, Queue<decimal>>();
            LastUpdate = new Dictionary<string, long>();
        }
    }


    
}
