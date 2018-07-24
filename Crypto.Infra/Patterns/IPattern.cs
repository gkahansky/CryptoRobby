﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;


namespace CryptoRobert.Infra.Patterns
{
    public interface IPattern
    {
        decimal HighPrice { get; set; }
        string Symbol { get; set; }
        string Interval { get; set; }
        string Name { get; set; }
        decimal DefaultStopLoss { get; }
        decimal DynamicStopLoss { get; }
        decimal DefaultStopLossThreshold { get; }
        decimal DynamicStopLossThreshold { get; }


        //bool CheckPattern(decimal price, long time);
        int CheckPattern(Kline kline);
        void SetHighPrice(decimal price);
        Pattern.PriceForCalc DefinePriceForCalculation(IPattern p);
        void UpdateSettings(PatternConfig config);
    }
}
