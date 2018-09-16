using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.RuleEngine.Interfaces;
using Crypto.RuleEngine.Entities;

namespace CryptoRobert.RuleEngine.Entities
{
    public abstract class RuleBase : IRule
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public int Retention { get; set; }
        public string RuleType { get; set; }
        public decimal Value { get; set; }
        public bool IsActive { get; set; }
        public List<RuleSet> RuleSets { get; set; }
        public Queue<Kline> Klines { get; set; }


        public RuleBase(string symbol, string interval, int retention, int id=0)
        {
            Symbol = symbol;
            Interval = interval;
            Retention = retention;
            Klines = new Queue<Kline>();
            RuleSets = new List<RuleSet>();
            Value = 0;
            Id = id;
        }

        public abstract void Calculate(Kline kline);

        public void ProcessKline(Kline kline)
        {
            if (Klines.Count() >= Retention)
            {
                Klines.Dequeue();
                Klines.Enqueue(kline);
            }
            else
                Klines.Enqueue(kline);
        }
    }
}
