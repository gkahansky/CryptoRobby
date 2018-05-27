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
        public List<string> Intervals { get; set; }

        public MetaData()
        {
            CoinDict = new Dictionary<string, Coin>();
            CoinPairDict = new Dictionary<string, CoinPair>();
            UserDict = new Dictionary<int, User>();
            Intervals = new List<string>() { "m1", "m3", "m5", "m15", "m30", "h1", "h2", "h4", "h6", "h8", "h12", "d1", "d3", "w1", "M1", "unknown" };
            
        }
    }
}
