using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Infra
{
    public class MetaData
    {
        public Dictionary<string, Coin> CoinDict { get; set; }
        public Dictionary<string, CoinPair> CoinPairDict { get; set; }
        public Dictionary<int, User> UserDict { get; set; }
        public Dictionary<string,long> Intervals { get; set; }

        public MetaData()
        {
            CoinDict = new Dictionary<string, Coin>();
            CoinPairDict = new Dictionary<string, CoinPair>();
            UserDict = new Dictionary<int, User>();
            Intervals = new Dictionary<string, long>();
            PopulateIntervals();
        }

        private void PopulateIntervals()
        {
            Intervals.Add("1m", 60000);
            Intervals.Add("3m", 180000);
            Intervals.Add("5m", 300000);
            Intervals.Add("15m", 900000);
            Intervals.Add("30m", 1800000);
            Intervals.Add("1h", 3600000);
            Intervals.Add("2h", 7200000);
            Intervals.Add("4h", 14400000);
            Intervals.Add("8h", 28800000);
            Intervals.Add("12h", 43200000);
            Intervals.Add("1d", 86400000);
            Intervals.Add("1w", 604800000);
            Intervals.Add("1M", 18144000000);
        }
    }
}
