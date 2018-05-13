using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core_org
{
    public static class MetaData
    {
        public static Dictionary<string, Coin> CoinDict { get; set; }
        public static Dictionary<string, CoinPair> CoinPairDict { get; set; }
        public static Dictionary<int, User> UserDict { get; set; }
        public static Dictionary<int, Wallet> WalletDict { get; set; }
    }
}
