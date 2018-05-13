using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core_org
{
    public class Coin
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public Coin(int id, string symbol, string name=null)
        {
            Id = Id;
            Symbol = symbol;
            Name = name;
        }
    }
}
