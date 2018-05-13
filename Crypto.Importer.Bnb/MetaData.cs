using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra
{
    public class MetaData
    {
        public Dictionary<string, Coin> CoinDict { get; set; }
        public Dictionary<string, CoinPair> CoinPairDict { get; set; }
        public Dictionary<int, User> UserDict { get; set; }

        public MetaData()
        {
            CoinDict = new Dictionary<string, Coin>();
            CoinPairDict = new Dictionary<string, CoinPair>();
            UserDict = new Dictionary<int, User>();
        }
    }
}
