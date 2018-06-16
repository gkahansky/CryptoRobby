﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;
using Crypto.RuleEngine.Patterns;

namespace Crypto.RuleEngine
{
    public interface IPattern
    {
        decimal HighPrice { get; set; }
        string Symbol { get; set; }
        string Interval { get; set; }
        string Name { get; set; }


        bool CheckPattern(decimal price, long time);
        bool CheckPattern(Kline kline);
        void SetHighPrice(decimal price);
        Pattern.PriceForCalc DefinePriceForCalculation(IPattern p);
        
    }
}