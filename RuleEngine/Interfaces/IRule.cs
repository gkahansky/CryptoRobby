using CryptoRobert.RuleEngine.Entities;
using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Interfaces
{
    public interface IRule
    {
        int Id { get; set; }
        string Key { get; set; }
        string Symbol { get; set; }
        string Interval { get; set; }
        int Retention { get; set; }
        decimal Value { get; set; }
        string RuleType { get; set; }
        decimal HighPrice { get; set; }

        void Calculate(Kline kline);

    }
}
