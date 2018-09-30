using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;
using Newtonsoft.Json.Linq;

namespace CryptoRobert.Infra.Patterns
{
    public class TrendInclinePattern : Pattern
    {
        private ILogger _logger;
        public decimal Trend { get; set; }
        public decimal avgPrice { get; set; }
        public decimal TrendIncline { get; set; }
        public decimal LastTrendIncline { get; set; }
        public decimal avgPriceChange { get; set; }
        public decimal lastAvgPrice { get; set; }
        public decimal avgChangePct { get; set; }
        public Queue<decimal> avgPriceList { get; set; }
        public decimal Pivot { get; set; }
        private DateTime TickTime { get; set; }
        private Dictionary<string, bool> Rules { get; set; }
        private Queue<decimal> PriceQueue { get; set; }

        public TrendInclinePattern(ILogger logger, PatternConfig settings, string engineName="Generic") : base(settings, logger, engineName)
        {
            Rules = new Dictionary<string, bool>();
            PriceQueue = new Queue<decimal>();
            avgPriceList = new Queue<decimal>();
            Retention = settings.Retention;
            Name = settings.Name;
            //Trend = 0;
            Threshold = settings.Threshold;
            avgPrice = 0;
            TrendIncline = 0;
            LastTrendIncline = 0;
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
            var buySell = 0;
            //1. Check Trend
            if (avgPrice > 0)
                buySell = CalculateIncline(avgPrice, lastAvgPrice, kline.Close);


            SetHighPrice(kline.Close);
            lastAvgPrice = avgPrice;
            LastTrendIncline = TrendIncline;

            return buySell;
        }

        private int CalculateIncline(decimal avgPrice, decimal lastAvgPrice, decimal price)
        {
            if (lastAvgPrice > 0)
            {
                var avgPriceDelta = avgPrice - lastAvgPrice;
                avgPriceChange = (avgPriceDelta / avgPrice);
                TrendIncline = CalculateTrendIncline(avgPriceChange);
            }

            if (TrendIncline > Threshold && price > avgPrice)
            {
                _logger.Warning(string.Format("BUY ALERT! Incline on the rise for {0} {1}!!! Current Trend: {2}, Last Trend {3}, Current Price: {4}, Time: {5}", Symbol, Interval, TrendIncline, LastTrendIncline, PriceQueue.Last(), TickTime));
                return 1;
            }
            else
                return -1;
        }

        private decimal CalculateTrendIncline(decimal avgPriceChange)
        {
            if (avgPriceList.Count() < 3)
            {
                avgPriceList.Enqueue(avgPriceChange);
                return 0;
            }
            else
            {
                avgPriceList.Dequeue();
                avgPriceList.Enqueue(avgPriceChange);
                var incline = avgPriceList.Average();
                return incline;
            }
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
