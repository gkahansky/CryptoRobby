using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Trading
{
    public class StopLossDefinition
    {
        public decimal DefaultStopLoss { get; set; }
        public decimal DefaultStopLossThreshold { get; set; }
        public decimal DynamicStopLoss { get; set; }
        public decimal DynamicSLThreshold { get; set; }
    }
}
