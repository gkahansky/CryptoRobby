using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core_org
{
    public class CoinPair
    {
        public string Symbol { get; set; }
        public int Coin1Id { get; set; }
        public int Coin2Id { get; set; }
        public decimal Price { get; set; }

        public CoinPair(string symbol, int coin1Id, int coin2Id, decimal price)
        {
            Symbol = symbol;
            Coin1Id = coin1Id;
            Coin2Id = coin2Id;
            Price = price;
        }
    }
}
