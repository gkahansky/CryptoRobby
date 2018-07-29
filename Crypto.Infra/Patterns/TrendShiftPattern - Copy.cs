using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;
using Newtonsoft.Json.Linq;

namespace CryptoRobert.Infra.Patterns
{
    public class TrendShiftPattern : Pattern
    {
        private ILogger _logger;
        public decimal Trend { get; set; }
        public int Retention { get; set; }
        public decimal Threshold { get; set; }
        public decimal avgPrice { get; set; }
        public decimal avgPriceDelta { get; set; }
        public decimal avgPriceChange { get; set; }
        public decimal lastAvgPrice { get; set; }
        public decimal avgChangePct { get; set; }
        public Queue<decimal> avgPriceList { get; set; }
        public decimal Pivot { get; set; }
        private DateTime TickTime { get; set; }
        private Dictionary<string, bool> Rules { get; set; }
        private Queue<decimal> PriceQueue { get; set; } 

        public TrendShiftPattern(ILogger logger, PatternConfig settings) : base(settings)
        {
            Rules = new Dictionary<string, bool>();
            PriceQueue = new Queue<decimal>();
            avgPriceList = new Queue<decimal>();
            Retention = settings.Retention;
            Name = settings.Name;
            Trend = 0;
            Threshold = settings.Threshold;
            avgPrice = 0;
            avgPriceDelta = 0;
            avgPriceChange = 0;
            lastAvgPrice = 0;
            _logger = logger;
            DefaultStopLossThreshold = settings.DefaultStopLoss;
            DynamicStopLossThreshold = settings.DynamicStopLoss;
        }

        public override int CheckPattern(Kline kline)
        {
            avgPrice = SaveKlineToPriceQueue(kline);
            var time = kline.CloseTime;

            TickTime = Parser.ConvertTimeMsToDateTime(time);


            //RunRules
            decimal newTrend = 0;
            //1. Check Trend
            if (avgPrice > 0)
            {
                newTrend = CalculateTrend(avgPrice, lastAvgPrice);
                avgChangePct = SaveAvgPriceChange(avgPriceChange);
                
            }
                

            //2. Compare to previous trend
            var buySell = CheckTrendShift(Trend, newTrend, time);

            lastAvgPrice = avgPrice;
            Trend = newTrend;

            return buySell;
        }

        private decimal SaveAvgPriceChange(decimal avgPriceChange)
        {
            decimal avg = 0;
            if (avgPriceList.Count() < 3)
            {
                avgPriceList.Enqueue(avgPriceChange);
            }
            else
            {
                avgPriceList.Dequeue();
                avgPriceList.Enqueue(avgPriceChange);
                avg = avgPriceList.Average();
            }
            return avg;
        }

        private decimal CalculateTrend(decimal avgPrice, decimal lastAvgPrice)
        {
            if (lastAvgPrice > 0)
            {
                avgPriceDelta = avgPrice - lastAvgPrice;
                avgPriceChange = (avgPriceDelta / avgPrice);
            }
            return avgPriceChange;
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

       
        private int CheckTrendShift(decimal lastTrend, decimal newTrend, long time)
        {
            if (avgChangePct > Threshold)
                return 1;
            else
                return -1;
            ////Trend was UP & newTrend DOWN
            //if (lastTrend > 0 && newTrend <= 0)
            //{
            //    _logger.Warning(string.Format("SELL ALERT! Trend Shift Detected for {0} {1}!!! Current Trend: {2}, Last Trend {3}, Current Price: {4}, Time: {5}", Symbol, Interval, newTrend, lastTrend, PriceQueue.Last(), TickTime));
            //    return -1;
            //}

                ////Trend was DOWN & newTrend UP
                //if (lastTrend <= 0 && newTrend > 0)
                //{
                //    _logger.Warning(string.Format("BUY ALERT! Trend Shift Detected for {0} {1}!!! Current Trend: {2}, Last Trend {3}, Current Price: {4}, Time: {5}", Symbol, Interval, newTrend, lastTrend, PriceQueue.Last(), TickTime));
                //    return 1;
                //}
                //else
                //    return 0;
        }

        public override PriceForCalc DefinePriceForCalculation(IPattern p)
        {
            return PriceForCalc.AvgClose;
        }

        public override void SetHighPrice(decimal price)
        {
            if (price > HighPrice)
                HighPrice = price;
        }

    }
}
