using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine.Patterns 
{
    public abstract class Pattern : IPattern
    {
        JObject Settings { get; set; }
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public string Name { get; set; }
        public decimal HighPrice { get; set; }
        public decimal DefaultStopLoss { get; set; }
        public decimal DefaultStopLossThreshold { get; set; }
        public decimal DynamicSLThreshold { get; set; }
        public decimal DynamicStopLoss { get; set; }

        public abstract bool CheckPattern(decimal avgPrice, long time);

        public abstract void SetHighPrice(decimal price);

    }
}
