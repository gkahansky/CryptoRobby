using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;

namespace CryptoRobert.Infra
{
    public class Kline
    {
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
        public long OpenTime { get; set; }
        public long CloseTime { get; set; }

        
    }

}
