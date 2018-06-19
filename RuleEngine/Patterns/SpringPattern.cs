using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;
using Newtonsoft.Json.Linq;

namespace Crypto.RuleEngine.Patterns
{
    public class SpringPattern : Pattern
    {
        private ILogger _logger;
        public bool Trend { get; set; }
        public decimal Threshold { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public decimal Spring { get; set; }
        public decimal LastPrice { get; set; }
        public decimal Pivot { get; set; }
        private DateTime TickTime { get; set; }
        private Dictionary<string, bool> Rules { get; set; }

        public SpringPattern(ILogger logger, PatternConfig settings) : base(settings)
        {
            Rules = new Dictionary<string, bool>();
            Name = settings.Name;
            ResetRules();
            Trend = false;
            Threshold = settings.Threshold;
            Low = 0;
            High = 0;
            Spring = 0;
            Pivot = 0;
            LastPrice = 0;
            _logger = logger;
            DefaultStopLossThreshold = settings.DefaultStopLoss;
            DynamicSLThreshold = settings.DynamicStopLoss;
        }

        public override bool CheckPattern(decimal avgPrice, long time)
        {
            TickTime = Parser.ConvertTimeMsToDateTime(time);
            //First data - Reset Params
            if (Low == 0 && High == 0 && Spring == 0)
            {
                ResetData(avgPrice);
                LastPrice = avgPrice;
                return false;
            }

            //RunRules
            //1. Wait for up trend and set low
            if (!Rules["Low"])
                Low = CheckLow(avgPrice, Low);
            //2. Wait for trend shift down and set high
            else if (Rules["Low"] && !Rules["High"])
                High = CheckHigh(avgPrice, High, Spring);
            //3. Wait for spring (Trend shift up), while staying above low
            else if (Rules["High"] && !Rules["Spring"])
                Spring = CheckSpring(avgPrice, Spring);
            //4. Wait to pass high
            else if (Rules["Spring"] && !Rules["Complete"])
                Pivot = CheckComplete(avgPrice, Spring, High, Pivot);

            else if (Rules["Complete"])
            {
                ResetData(Spring);
                return true;
            }

            _logger.Log(String.Format("Low: {0}, High: {1}, Spring: {2}, Last Price: {3}, Highest Price: {4}, time: {5}", Low, High, Spring, LastPrice, HighPrice, TickTime));
            LastPrice = avgPrice;
            return false;
        }

        public override void SetHighPrice(decimal price)
        {
            if (price > HighPrice)
                HighPrice = price;
        }

        private decimal CheckLow(decimal avgPrice, decimal low)
        {
            var lowTop = low + (low * Threshold);
            var lowBottom = low - (low * Threshold);
            //Price goes down
            if (avgPrice < low)
            {
                low = avgPrice;
                _logger.Log(string.Format("LOW Price update: {0}, Top Range: {1}, Bottom Range: {2}", low, lowTop, lowBottom));
                if (avgPrice < lowBottom)
                {
                    Trend = false;
                    Rules["Low"] = false;
                }
            }
            //Price goes up
            else if (avgPrice > lowTop)
            {
                Trend = true;
                Rules["Low"] = true;
                High = avgPrice;
                _logger.Log(String.Format("LOW RULE ACHIEVED!!! Moving to HIGH Rule"));
            }
            return low;
        }

        private decimal CheckHigh(decimal avgPrice, decimal high, decimal spring)
        {
            var highTop = high + (high * Threshold);
            var highBottom = high - (high * Threshold);

            if (avgPrice > high)
            {
                high = avgPrice;
                _logger.Log(string.Format("HIGH Price update: {0}, Top Range: {1}, Bottom Range: {2}", high, highTop, highBottom));
                if (avgPrice > highTop)
                    Trend = true;
            }
            else if (avgPrice < highBottom)
            {
                Trend = false;
                //Spring = price;
                Spring = High;
                Rules["High"] = true;
                _logger.Log(String.Format("HIGH RULE ACHIEVED!!! Moving to SPRING Rule"));
            }
            return high;

        }

        private decimal CheckSpring(decimal avgPrice, decimal high)
        {
            var springFloor = Low;
            var springTop = Spring + (Spring * Threshold);
            var springBottom = Spring - (Spring * Threshold);


            //Trend shift up - Spring is set, Rule is complete
            if (avgPrice > springTop)
            {
                Rules["Spring"] = true;
                Trend = true;
                Pivot = avgPrice;
                _logger.Log(String.Format("SPRING RULE ACHIEVED!!! Moving to COMPLETION Rule"));
            }
            else if (avgPrice < Spring)
            {
                if (avgPrice > springFloor)
                {
                    Spring = avgPrice;
                    _logger.Log(string.Format("SPRING Price update: {0}, Top Range: {1}, Bottom Range: {2}, Limit for pattern break: {3}", Spring, springTop, springBottom, springFloor));
                }
                    
                else if (avgPrice < springFloor)
                {
                    ResetData(avgPrice);
                }
            }

            return Spring;
        }

        private decimal CheckComplete(decimal avgPrice, decimal spring, decimal high, decimal pivot)
        {
            var highTop = high + (high * Threshold);
            var springBottom = spring - (spring * Threshold);
            var pivotHigh = pivot + (pivot * Threshold);
            var pivotLow = pivot - (pivot * Threshold);

            if (avgPrice >= high)
            {
                Rules["Complete"] = true;
                _logger.Log(String.Format("SPRING PATTERN DETECTED - BUY NOW! {0}", TickTime));
            }
            else if (avgPrice > pivot)
            {
                pivot = avgPrice;
            }
            else if (avgPrice < pivotLow)
            {
                Trend = false;
                ResetData(avgPrice);
            }

            return pivot;
        }

        private void ResetData(decimal avgPrice)
        {
            Low = avgPrice;
            High = avgPrice;
            Spring = avgPrice;
            ResetRules();
        }

        private void ResetRules()
        {
            Rules.Clear();
            Rules.Add("Low", false);
            Rules.Add("High", false);
            Rules.Add("Spring", false);
            Rules.Add("Complete", false);
        }

        public override PriceForCalc DefinePriceForCalculation(IPattern p)
        {
            return PriceForCalc.AvgClose;
        }

        public override bool CheckPattern(Kline kline)
        {
            throw new NotImplementedException();
        }
    }
}
