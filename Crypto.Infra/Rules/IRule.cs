using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra.Rules
{
    public interface IRule
    {
        string Symbol { get; set; }
        string Interval { get; set; }
        int Retention { get; set; }
        int Priority { get; set; }
        bool IsAchieved { get; set; }


        bool CheckRule(Kline kline);

    }
}
