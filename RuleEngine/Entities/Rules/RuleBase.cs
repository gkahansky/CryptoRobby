using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.RuleEngine.Entities;

namespace CryptoRobert.RuleEngine.Entities.Rules
{
    public abstract class RuleBase : IRule
    {
        public int          Id          { get; set; }
        public string       Key         { get; set; }
        public string       Symbol      { get; set; }
        public string       Interval    { get; set; }
        public int          Retention   { get; set; }
        public string       RuleType    { get; set; }
        public decimal      Value       { get; set; }
        public bool         IsActive    { get; set; }
        public Queue<Kline> Klines      { get; set; }


        public RuleBase(string symbol, string interval, int retention, int id=0)
        {
            Symbol      = symbol;
            Interval     = interval;
            Retention   = retention;
            Klines       = new Queue<Kline>();
            Value        = 0;
            Key          = GenerateKey();
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

        private string GenerateKey()
        {
            string[] attributes = { this.Symbol, this.Interval, this.Retention.ToString(), this.RuleType };
            string key = string.Join("_", attributes);
            return key;
        }
    }
}
