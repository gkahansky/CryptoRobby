using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra
{
    public class Coin
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public Coin(string symbol, string name = null)
        {
            Symbol = symbol;
            Name = name;
        }

        public Coin()
        {

        }
    }
}
