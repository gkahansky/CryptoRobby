using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core_org
{
    public class EnumCollection
    {
        public enum Exchange { Binance }
        public enum RuleType { CoinValue, MarketCap, Trend }
        public enum Condition { Above, Below, Equals }
        public enum ValueType { BTC, USD, ETH, BNB }
        public enum OrderType { Buy, Sell }
        public enum RateType { Limit, Market }
        public enum UserRole { Admin, User }
    }
}
