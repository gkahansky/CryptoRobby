using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra
{
    public class PatternConfig
    {
        public string Name { get; set; }
        public decimal Threshold { get; set; }
        public int Retention { get; set; }
        public decimal DefaultStopLoss { get; set; }
        public decimal DynamicStopLoss { get; set; }
        public bool IsActive { get; set; }
        public string Symbol { get; set; }
        public string Interval { get; set; }
    }
}
