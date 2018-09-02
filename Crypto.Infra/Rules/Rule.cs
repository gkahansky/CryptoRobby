using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra.Rules
{
    public abstract class Rule : IRule
    {
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public int Retention { get; set; }
        public int Priority { get; set; }
        public bool IsAchieved { get; set; }
        public IAction Action  { get; set; }
        public DataRepository Repository { get; set; }
        public decimal Threshold { get; set; }
        public decimal AvgPrice { get; set; }
        public decimal AvgVolume { get; set; }
        public decimal LastAvgPrice { get; set; }
        public decimal LastAvgVolume { get; private set; }
        public decimal LastPrice { get; set; }
        public decimal LastVolume { get; set; }

        public Rule(string symbol, string interval, int retention, decimal threshold, int priority = 0, IAction action=null)
        {
            Symbol = symbol;
            Interval = interval;
            Retention = retention;
            Action = action;
            Threshold = threshold;
            Priority = priority;
            Repository = new DataRepository();
            IsAchieved = false;
        }

        public abstract bool CheckRule(Kline kline);
        
        public void ProcessKline(Kline kline)
        {
            if (Repository.Klines.Count() > 0)
            {
                LastPrice = Repository.Klines.Peek().Close;
                LastVolume = Repository.Klines.Peek().Volume;
            }
            

            if (Repository.Klines.Count() >= Retention)
            {
                Repository.Klines.Dequeue();
                Repository.Klines.Enqueue(kline);
            }
            else
                Repository.Klines.Enqueue(kline);

            LastAvgPrice = AvgPrice;
            LastAvgVolume = AvgVolume;
            AvgPrice = CheckAvgPrice();
            AvgVolume = CheckAvgVolume();
        }

        private decimal CheckAvgPrice()
        {
            decimal avg = 0;
            decimal sum = 0;

            foreach (var kline in Repository.Klines)
            {
                sum += kline.Close;
            }

            avg = sum / Repository.Klines.Count();
            return avg;
        }

        private decimal CheckAvgVolume()
        {
            decimal avg = 0;
            decimal sum = 0;

            foreach (var kline in Repository.Klines)
            {
                sum += kline.Volume;
            }

            avg = sum / Repository.Klines.Count();
            return avg;
        }
    }
}
