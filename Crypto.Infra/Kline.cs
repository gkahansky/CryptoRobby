using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;

namespace Crypto.Infra
{
    public class Kline
    {
        public EnumCollection.KlineInterval Interval { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
        public long OpenTime { get; set; }
        public long CloseTime { get; set; }
    }

}
