using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Trading
{
    public class PriceRepository
    {
        private Dictionary<string,decimal> Prices { get; set; }

        public PriceRepository()
        {
            Prices = new Dictionary<string, decimal>();
        }

        public void UpdatePrice(string pair, decimal price)
        {
            if (Prices.ContainsKey(pair))
            {
                Prices[pair] = price;
            }
            else
            {
                Prices.Add(pair, price);
            }
        }

        public decimal UpdateHighPrice(decimal highPrice, decimal price)
        {
            if (highPrice < price)
                highPrice = price;
            return highPrice;
        }
    }
}
