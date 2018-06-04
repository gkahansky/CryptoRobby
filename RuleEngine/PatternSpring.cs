using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;
using Newtonsoft.Json.Linq;

namespace Crypto.RuleEngine
{
    public class PatternSpring : IPattern
    {
        private ILogger _logger;
        public string Symbol { get; set; }
        public string Interval { get; set; }
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
        public bool TrendValid { get; set; }

        public PatternSpring(ILogger logger, JObject config)
        {
            _logger = logger;
            //Symbol = config["Symbol"].ToString();
            //Interval = config[Interval].ToString();
            H1 = 0;
            H2 = 0;
            L1 = 0;
            L2 = 0;
            LastPrice = 0;
            Pivot = 0;
            Margin = Config.PatternSpringThreshold;
            Trend = false;
            TrendShift = false;
            TrendValid = false;
        }

        public bool CheckPattern(decimal cp, long time)
        {
            var result = false;
            var timeDate = Parser.ConvertTimeMsToDateTime(time);

            if (H1 == 0 & H2 == 0 & L1 == 0 & L2 == 0)
            {
                H1 = cp;
                H2 = cp;
                L1 = cp;
                L2 = cp;
                LastPrice = cp;
                //Pivot = cp; //Erase if going back to previous function
                return result;
            }

            if (cp - H1 > (H1 * Margin)) //strict
            //if (Math.Abs((cp - H1) / H1) < Margin) //lean
            {
                if (L1 - H2 > H2 * Margin)
                {
                    if (H1 - H2 > H2 * Margin)
                    {
                        if (!TrendValid)
                            _logger.Log("**** Pattern Achieved!!! Buy Now at " + cp + " priceTime: " + timeDate.ToString()  + " ****");
                        result = true;
                        TrendValid = true;
                    }
                }
            }
            CalculatePatternPrices(cp);

            _logger.Log(String.Format("Current Price: {0}, Last Price {1}, H1: {2}, H2: {3}, L1: {4}, L2: {5}", cp, LastPrice, H1, H2, L1, L2));
            LastPrice = cp;
            if (!result)
                TrendValid = false;
            return result;

        }

        private void CalculatePatternPrices(decimal cp)
        {
            if (Pivot == 0)
            {
                if (cp > H1)
                {
                    Pivot = cp;
                    Trend = true;
                }
                else if (cp < L1)
                {
                    Pivot = cp;
                    Trend = false;
                }

            }

            else if (cp / Pivot > Margin + 1) // Higher than H1
            {
                if (Trend) // Trend up cont.
                {
                    if (Pivot < cp)
                        Pivot = cp;
                }
                else if (!Trend) //trending up after downtrend
                {
                    L2 = L1;
                    L1 = LastPrice;
                    Trend = true;
                    TrendShift = true;
                    Pivot = cp;
                }
            }
            else if (Pivot / cp > Margin + 1) //Lower than L1
            {
                if (!Trend)
                {
                    if (Pivot > cp)
                        Pivot = cp;
                }
                else if (Trend)
                {
                    H2 = H1;
                    H1 = cp;
                    Trend = false;
                    TrendShift = true;
                    Pivot = cp;
                }
            }
        }
    }
}
