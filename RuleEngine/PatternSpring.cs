using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;

namespace Crypto.RuleEngine
{
    public class PatternSpring : IPattern
    {
        private ILogger _logger;
        private decimal InitialPrice { get; set; }
        public decimal H1 { get; set; }
        public decimal H2 { get; set; }
        public decimal L1 { get; set; }
        public decimal L2 { get; set; }
        public decimal LastPrice { get; set; }
        public decimal Margin { get; set; }
        public decimal Pivot { get; set; }
        public bool Trend { get; set; }
        public bool TrendShift { get; set; }

        public PatternSpring(ILogger logger)
        {
            _logger = logger;
            H1 = 0;
            H2 = 0;
            L1 = 0;
            L2 = 0;
            LastPrice = 0;
            Pivot = 0;
            Margin = 0.02m;
            Trend = false;
            TrendShift = false;
        }

        public bool CheckPattern(decimal cp)
        {
            var result = false;

            if (H1 == 0 & H2 == 0 & L1 == 0 & L2 == 0)
            {
                H1 = cp;
                H2 = cp;
                L1 = cp;
                L2 = cp;
                LastPrice = cp;
                return result;
            }

            if (cp-H1 > (H1 * Margin))
            {
                if (L1 - H2 > H2 * Margin)
                {
                    if (H1 - H2 > H2 * Margin)
                    {
                        _logger.Log("**** Pattern Achieved!!! Buy Now at " + cp + " ****");
                        result = true;
                    }
                }
            }
            CalculatePatternPrices(cp);

            _logger.Log(String.Format("Current Price: {0}, Last Price {1}, H1: {2}, H2: {3}, L1: {4}, L2: {5}", cp, LastPrice, H1, H2, L1, L2));
            LastPrice = cp;

            return result;

        }



        private void CalculatePatternPrices(decimal cp)
        {
            var L1Over = L1 + (L1 * Margin);
            var L1Under = L1 - (L1 * Margin);
            var H1Over = H1 + (H1 * Margin);
            var H1Under = H1 - (H1 * Margin);
            var lastOver = LastPrice + (LastPrice * Margin);
            var lastUnder = LastPrice - (LastPrice * Margin);


            if(L1==L2 && H1 == H2 && LastPrice == H1) //first time
            {
                if (cp > H1Over)
                {
                    Trend = true;
                    TrendShift = false;
                }
                else if (cp < L1Under)
                {
                    Trend = false;
                    TrendShift = false;
                }
            }

            //5.4m, 5, 6.1m, 6.11m, 7, 6.998m, 6.9m, 6.5m, 6.55m, 6.6m, 6.7m, 6.9m, 6.997m, 6.99m, 7.1m, 7.5

            else if (cp > LastPrice) // cp greater than before
            {
                if (Trend) // Trend continues
                {
                    if(cp > H1Over) // H1 rises
                    {
                        TrendShift = false;
                    }
                }
                else if (!Trend) //We are currently trending down and a higher price arrived
                {
                    if(Pivot == 0) // first positive after downtrend
                    { 
                        if(cp > lastOver) // new price above last and ABOVE margin
                        {
                            L2 = L1;
                            L1 = LastPrice;
                            Trend = true;
                            TrendShift = true;
                        }
                        else if (cp <= lastUnder) // new price above last but BELOW margin
                        {
                            Pivot = LastPrice;
                            TrendShift = false;
                        }
                    }
                    else // trend is down, previous price was positive but below margin
                    {
                        if (cp - Pivot > Pivot * Margin) // new high ABOVE margin, trend shift up, update lows & reset pivot
                        {
                            L2 = L1;
                            L1 = LastPrice;
                            Pivot = 0;
                            Trend = true;
                            TrendShift = true;
                        }
                        else if (cp - Pivot < Pivot * Margin) // new high BELOW margin
                        {
                            TrendShift = false;
                        }
                    }
                }
            }
            else if(cp <= LastPrice) // price goes down
            {
                if (!Trend) // Price in downtrend and continues to drop
                {
                    if(cp < L1Under)
                    {
                        L1 = cp;
                        TrendShift = false;
                    }
                }
                else if (Trend) // price in up trend and a low price arrived
                {
                    if(Pivot == 0) // first time a low price arrives after up trend
                    {
                        if(cp < lastUnder) //low price sets new trend, update Highs 
                        {
                            H2 = H1;
                            H1 = LastPrice;
                            Trend = false;
                            TrendShift = true;
                        }
                        else // low price not enough to change trend. setting Pivot
                        {
                            Pivot = LastPrice;
                            TrendShift = false;
                        }
                    }
                    else // low price arrives during downtrend but not first time.
                    {
                        if(Pivot - cp > Pivot * Margin) //low price sets new trend, update Highs and reset pivot
                        {
                            H2 = H1;
                            H1 = LastPrice;
                            Pivot = 0;
                            Trend = false;
                            TrendShift = true;
                        }
                        else
                        {
                            TrendShift = false;
                        }
                    }
                }
            }
        }
       
    }
}
