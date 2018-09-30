using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Infra.Patterns
{
    public class PatternView
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public string Name { get; set; }
        public decimal DefaultStopLossThreshold { get; set; }
        public decimal DynamicStopLossThreshold { get; set; }

        public PatternView(IPattern p)
        {
            Name = p.Name;
            Symbol = p.Symbol;
            Interval = p.Interval;
            DefaultStopLossThreshold = p.DefaultStopLossThreshold;
            DynamicStopLossThreshold = p.DynamicStopLossThreshold;
        }

    }
}
