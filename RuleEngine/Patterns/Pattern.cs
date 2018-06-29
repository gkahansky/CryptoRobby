﻿using CryptoRobert.Infra;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Patterns 
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
        public decimal DynamicStopLossThreshold { get; set; }
        public decimal DynamicStopLoss { get; set; }
        public enum PriceForCalc { AvgClose, Close, High, Low, Open, AvgOC, avgHL }

        public Pattern(PatternConfig settings)
        {
            Symbol = settings.Symbol;
            Interval = settings.Interval;
            Name = settings.Name;
        }

        public void UpdateSettings(PatternConfig config)
        {
            this.Name = config.Name;
            this.Interval = config.Interval;
            this.DefaultStopLossThreshold = config.DefaultStopLoss;
            this.DynamicStopLoss = config.DynamicStopLoss;
            this.Symbol = config.Symbol;
        }

        //public abstract bool CheckPattern(decimal avgPrice, long time);

        public abstract bool CheckPattern(Kline kline);

        public abstract void SetHighPrice(decimal price);

        public abstract PriceForCalc DefinePriceForCalculation(IPattern p);

    }
}
