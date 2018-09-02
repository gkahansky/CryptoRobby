using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra.MarketData
{
    public class TickGap
    {
        public long From { get; set; }
        public long To { get; set; }

        public TickGap(long from, long to)
        {
            From = from;
            To = to;
        }

    }
}
