using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Infra
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

        public PatternConfig()
        {
        }

        public PatternConfig(string name, string symbol, string interval, bool IsActive = false, decimal threshold = 0.03m, int retention=10, decimal defaultSl=0.05m, decimal dynamicSl=0.05m )
        {
            Name = name;
            Symbol = symbol;
            Interval = interval;
            IsActive = IsActive;
            Threshold = threshold;
            Retention = retention;
            DefaultStopLoss = defaultSl;
            DynamicStopLoss = dynamicSl;
        }
    }
}
