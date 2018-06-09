using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;

namespace Crypto.RuleEngine
{
    public class PatternEngine
    {
        Dictionary<string, CoinPair> CoinPairDict { get; set; }
        PatternFactory Factory { get; set; }
        public List<IPattern> Patterns { get; set; }
        ILogger _logger;

        public PatternEngine(ILogger logger)
        {
            _logger = logger;
            CoinPairDict = new Dictionary<string, CoinPair>();
            Patterns = new List<IPattern>();
            Factory = new PatternFactory(_logger);
        }

        public void CalculatePatternsFromDataFeed(List<Kline> klineList, List<IPattern> patterns)
        {
            if (klineList.Count > 0)
            {
                foreach (var kline in klineList)
                {
                    var price = kline.Close;// (kline.Low + kline.High) / 2;
                    var interval = kline.Interval;

                    // pair doesn't exist in dictionary. initialize New Pair in dictionary + new queue for interval.
                    if (!CoinPairDict.ContainsKey(kline.Symbol)) 
                    {
                        InitializeCoinPair(kline.Symbol, interval);
                    }
                    // Pair exists, interval doesn't exist, initialize new queue and add to dict.
                    else if (!CoinPairDict[kline.Symbol].LastPrices.ContainsKey(interval)) 
                    {
                        var queue = new Queue<decimal>();
                        CoinPairDict[kline.Symbol].LastPrices.Add(interval, queue);
                    }

                    //Push new price and calculate new average
                    AddPriceToQueue(CoinPairDict[kline.Symbol], kline, price);

                    //Check prerequisites and run pattern calculation if relevant.
                    if (CoinPairDict[kline.Symbol].AvgPrice > 0)
                    {
                        foreach (var p in patterns)
                        {
                            var buy = p.CheckPattern(CoinPairDict[kline.Symbol].AvgPrice, kline.OpenTime);
                        }
                    }
                }
            }
            else
                _logger.Log("No Relevant Data found for analysis. Please check your query parameters");
        }

        private void InitializeCoinPair(string symbol, string interval)
        {
            var pair = new CoinPair();
            pair.AvgPrice = 0;
            pair.LastPrices = new Dictionary<string, Queue<decimal>>();
            pair.Symbol = symbol;
            var queue = new Queue<decimal>();
            pair.LastPrices.Add(interval, queue);
            CoinPairDict.Add(pair.Symbol, pair);
        }

        private void GeneratePatterns(string[] patterns)
        {
            Factory = new PatternFactory(_logger);
            Patterns = new List<IPattern>();

            foreach (var p in patterns)
            {
                var newPattern = Factory.CreateInstance(p);
                if (newPattern != null)
                    Patterns.Add(newPattern);
            }
        }

        private void AddPriceToQueue(CoinPair pair, Kline kline, decimal price)
        {
            if (pair.LastPrices.ContainsKey(kline.Interval))
            {
                //check if queue is full
                if (pair.LastPrices[kline.Interval].Count() == Config.PatternSpringToKeep)
                {
                    pair.LastPrices[kline.Interval].Dequeue();
                    pair.LastPrices[kline.Interval].Enqueue(price);
                    pair.AvgPrice = CalculateAveragePrice(pair.LastPrices[kline.Interval]);
                    pair.AvgPriceOpenTime = kline.OpenTime;
                }
                else // queue is not full
                {
                    pair.LastPrices[kline.Interval].Enqueue(price);
                }

            }
        }

        private decimal CalculateAveragePrice(Queue<decimal> queue)
        {
            decimal price = 0;
            foreach (var item in queue)
            {
                price += item;
            }

            var avgPrice = price / queue.Count();
            return avgPrice;
        }
    }
}
