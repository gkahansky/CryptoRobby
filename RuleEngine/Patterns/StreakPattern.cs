﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;
using Newtonsoft.Json.Linq;

namespace Crypto.RuleEngine.Patterns
{
    public class StreakPattern : Pattern
    {
        private ILogger _logger;
        public int Streak { get; set; }
        public decimal LastPrice { get; set; }
        public decimal LastOpen { get; set; }
        private Queue<decimal> PriceQueue { get; set; }
        private Queue<decimal> VolumeQueue { get; set; }
        private int Retention { get; set; }
        private decimal StreakThreshold { get; set; }

        public StreakPattern(ILogger logger, JObject settings) : base(settings)
        {
            _logger = logger;
            Streak = 0;
            LastPrice = 0;
            LastOpen = 0;
            PriceQueue = new Queue<decimal>();
            VolumeQueue = new Queue<decimal>();
            Retention = int.Parse(settings["Retention"].ToString());
            HighPrice = 0;
            StreakThreshold = decimal.Parse(settings["Threshold"].ToString());
            Name = "Streak";
        }

        public override bool CheckPattern(Kline kline)
        {
            var price = kline.Close;
            var volume = kline.Volume;
            var time = long.Parse(kline.CloseTime.ToString());
            var TickTime = Parser.ConvertTimeMsToDateTime(time);
            var result = false;

            if (LastPrice == 0 || PriceQueue.Count < Retention)
            {
                LastPrice = kline.Close;
                LastOpen = kline.Open;
                PriceQueue.Enqueue(price);
                VolumeQueue.Enqueue(volume);
                return false;
            }

            var avgPrice = PriceQueue.Average();
            var sd = CalculateSd(VolumeQueue);

            var priceToBeat = LastPrice + (LastPrice * StreakThreshold);

            if (PriceQueue.Count >= Retention)
            {
                PriceQueue.Dequeue();
                PriceQueue.Enqueue(price);
                VolumeQueue.Dequeue();
                VolumeQueue.Enqueue(volume);

                if (kline.Open > avgPrice && kline.Open > LastOpen && price > priceToBeat)
                {
                    Streak += 1;
                    var msg = String.Format("Streak Forming! Average Price: {0}, Current Price: {1}, Last Price: {2} Streak: {3}, Time: {4}", avgPrice, price, LastPrice, Streak, TickTime);
                    _logger.Log(msg);
                }
                else
                {
                    Streak = 0;
                    var msg = String.Format("Streak Ended! Average Price: {0}, Current Price: {1}, Last Price: {2} Streak: {3}, Time: {4}", avgPrice, price, LastPrice, Streak, TickTime);
                    _logger.Log(msg);
                }
            }

            if (Streak == 3 && volume > 2*sd)
            {
                result = true;
                var msg = String.Format("Streak Achieved! Average Price: {0}, Current Price: {1}, Last Price: {2} Streak: {3}, Time: {4}", avgPrice, price, LastPrice, Streak, TickTime);
                _logger.Log(msg);
            }

            LastPrice = kline.Close;
            LastOpen = kline.Open;

            return result;

        }


        private decimal CalculateSd(Queue<decimal> queue)
        {
            var avg = queue.Average();
            var sumOfSquaresOfDifferences = queue.Select(val => (val - avg) * (val - avg)).Sum();
            var sumOfSqaureDiffDouble = (double)sumOfSquaresOfDifferences;
            var sd = (decimal)Math.Sqrt(sumOfSqaureDiffDouble / queue.Count);
            return sd;
            
        }

        public override void SetHighPrice(decimal price)
        {
            if (price > HighPrice)
                HighPrice = price;
        }

        public override PriceForCalc DefinePriceForCalculation(IPattern p)
        {
            return PriceForCalc.Close;
        }

        public override bool CheckPattern(decimal price, long time)
        {
            throw new NotImplementedException();
        }
    }
}