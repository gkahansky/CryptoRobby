using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine
{
    public interface IPattern
    {
        decimal HighPrice { get; set; }
        string Symbol { get; set; }
        string Interval { get; set; }
        string Name { get; set; }

        bool CheckPattern(decimal price, long time);
        void SetHighPrice(decimal price);
    }
}
