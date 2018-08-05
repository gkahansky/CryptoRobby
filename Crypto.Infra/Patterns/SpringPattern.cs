using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;
using Newtonsoft.Json.Linq;

namespace CryptoRobert.Infra.Patterns
{
    public class SpringPattern : Pattern
    {
        private ILogger _logger;
        public bool Trend { get; set; }
        public int Retention { get; set; }
        public decimal Threshold { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public decimal Spring { get; set; }
        public decimal LastPrice { get; set; }
        public decimal Pivot { get; set; }
        private DateTime TickTime { get; set; }
        private Dictionary<string, bool> Rules { get; set; }
        private Queue<decimal> PriceQueue { get; set; } 

        public SpringPattern(ILogger logger, PatternConfig settings, string engineName = "Generic") : base(settings, logger, engineName)
        {
            Rules = new Dictionary<string, bool>();
            PriceQueue = new Queue<decimal>();
            Retention = settings.Retention;
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
            DynamicStopLossThreshold = settings.DynamicStopLoss;
        }

        public override int CheckPattern(Kline kline)
        {
            var avgPrice = SaveKlineToPriceQueue(kline);
            var time = kline.CloseTime;

            TickTime = Parser.ConvertTimeMsToDateTime(time);
            //First data - Reset Params
            if (Low == 0 && High == 0 && Spring == 0)
            {
                ResetData(avgPrice);
                LastPrice = avgPrice;
                return 0;
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
                return 1;
            }

            _logger.Debug(String.Format("{6} {7}: Low: {0}, High: {1}, Spring: {2}, Last Price: {3}, Highest Price: {4}, time: {5}", Low, High, Spring, LastPrice, HighPrice, TickTime, this.Symbol, this.Interval));
            LastPrice = avgPrice;
            return 0;
        }

        private decimal SaveKlineToPriceQueue(Kline kline)
        {
            if (PriceQueue.Count() < this.Retention)
            {
                PriceQueue.Enqueue(kline.Close);
                return 0;
            }
            else
            {
                PriceQueue.Dequeue();
                PriceQueue.Enqueue(kline.Close);
                var avg = PriceQueue.Average();
                return avg;
            }
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
                _logger.Debug(string.Format("LOW Price update {3} {4}: {0}, Top Range: {1}, Bottom Range: {2}", low, lowTop, lowBottom, this.Symbol, this.Interval));
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
                _logger.Debug(String.Format("LOW RULE ACHIEVED {0} {1}!!! Moving to HIGH Rule", this.Symbol, this.Interval));
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
                _logger.Debug(string.Format("HIGH Price update {3} {4}: {0}, Top Range: {1}, Bottom Range: {2}", high, highTop, highBottom, this.Symbol, this.Interval));
                if (avgPrice > highTop)
                    Trend = true;
            }
            else if (avgPrice < highBottom)
            {
                Trend = false;
                //Spring = price;
                Spring = High;
                Rules["High"] = true;
                _logger.Debug(String.Format("HIGH RULE ACHIEVED {0} {1}!!! Moving to SPRING Rule", this.Symbol, this.Interval));
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
                _logger.Debug(String.Format("SPRING RULE ACHIEVED {0} {1}!!! Moving to COMPLETION Rule", this.Symbol, this.Interval));
            }
            else if (avgPrice < Spring)
            {
                if (avgPrice > springFloor)
                {
                    Spring = avgPrice;
                    _logger.Debug(string.Format("SPRING Price update {4} {5}: {0}, Top Range: {1}, Bottom Range: {2}, Limit for pattern break: {3}", Spring, springTop, springBottom, springFloor, this.Symbol, this.Interval));
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
                _logger.Info(String.Format("SPRING PATTERN DETECTED - BUY {0} NOW! {1}", this.Symbol, TickTime));
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

        
    }
}
