using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra
{
    public static class EnumCollection
    {
        public enum Exchange { Binance }
        public enum RuleType { CoinValue, MarketCap, Trend }
        public enum Condition { Above, Below, Equals }
        public enum ValueType { BTC, USD, ETH, BNB }
        public enum OrderType { Buy, Sell }
        public enum RateType { Limit, Market }
        public enum UserRole { Admin, User }
        public enum baseUnit { Dollar, LeftCoin, RightCoin}
        public enum KlineInterval { m1, m3, m5, m15, m30, h1, h2, h4, h6, h8, h12, d1, d3, w1, M1, unknown }


        public static KlineInterval ConvertIntervalStringToEnum(string interval)
        {
            switch (interval)
            {
                case "1m":
                    return KlineInterval.m1;
                case "3m":
                    return KlineInterval.m3;
                case "5m":
                    return KlineInterval.m5;
                case "15m":
                    return KlineInterval.m15;
                case "30m":
                    return KlineInterval.m30;
                case "1h":
                    return KlineInterval.h1;
                case "2h":
                    return KlineInterval.h2;
                case "4h":
                    return KlineInterval.h4;
                case "6h":
                    return KlineInterval.h6;
                case "8h":
                    return KlineInterval.h8;
                case "12h":
                    return KlineInterval.h12;
                case "1d":
                    return KlineInterval.d1;
                case "3d":
                    return KlineInterval.d3;
                case "1w":
                    return KlineInterval.w1;
                case "1M":
                    return KlineInterval.M1;
                default:
                    return KlineInterval.unknown;
            }
        }
    }

    
}

    