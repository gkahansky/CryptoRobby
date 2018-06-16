﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;
using Crypto.RuleEngine.Transactions;
using Crypto.RuleEngine.Patterns;
using Newtonsoft.Json.Linq;

namespace Crypto.RuleEngine
{
    public class PatternEngine
    {
        public Dictionary<string, CoinPair> CoinPairDict { get; set; }
        public Dictionary<string, Transaction> Transactions { get; set; }
        PatternFactory Factory { get; set; }
        public PatternRepository Patterns { get; set; }
        public Dictionary<string, StopLossDefinition> StopLossCollection { get; set; }
        ILogger _logger;

        public PatternEngine(ILogger logger)
        {
            _logger = logger;
            CoinPairDict = new Dictionary<string, CoinPair>();
            Factory = new PatternFactory(_logger);
            Patterns = new PatternRepository();
            StopLossCollection = new Dictionary<string, StopLossDefinition>();
            Transactions = new Dictionary<string, Transaction>();
        }

        public void CheckAllPatterns(List<Kline> klineList, Dictionary<string, IPattern> patterns, Dictionary<string, JObject> patternsConfig)
        {
            if (klineList.Count > 0)
            {
                foreach (var kline in klineList)
                {
                    //var price = kline.Close;
                    var interval = kline.Interval;
                    var sell = false;
                    SavePriceToQueue(CoinPairDict, kline);


                    //Check prerequisites and run pattern calculation if relevant.
                    if (CoinPairDict[kline.Symbol].AvgPrice > 0)
                    {
                        foreach (var pattern in patterns)
                        {
                            var p = pattern.Value;
                            var patternPrice = SetPriceForPattern(p, kline);
                            var buy = CheckPatterns(p, kline);
                            p.SetHighPrice(kline.High);

                            if (buy)
                                BuyPair(kline, patternsConfig, p.Name);

                            else
                                if (Transactions.Count > 0)
                                sell = CheckStopLoss(p, kline);

                            if (sell)
                                Sell(kline.Symbol, kline.Close);

                        }
                    }
                }
            }
            else
                _logger.Log("No Relevant Data found for analysis. Please check your query parameters");
        }

        private void Sell(string symbol, decimal price)
        {
            var t = Transactions[symbol];
            var profit = ((price / t.BuyPrice) - 1) * 100;
            var profitText = profit.ToString();
            if (profitText.Length > 5)
                profitText = profit.ToString().Substring(0, 5);
            var msg = string.Format("Trade: SELLING {0}!!! Buy Price: {1}, Sell Price: {2}, Profit: {3}%", t.Symbol, t.BuyPrice, price.ToString(), profitText);
            _logger.Log(msg);
            Transactions.Remove(symbol);
        }

        private bool CheckStopLoss(IPattern p, Kline kline)
        {

            if (!Transactions.ContainsKey(kline.Symbol))
                return false;

            var t = Transactions[kline.Symbol];
            t.HighPrice = p.HighPrice;

            var sell = t.CheckStopLoss(t, kline.Close);

            return sell;
        }

        private void BuyPair(Kline kline, Dictionary<string, JObject> patternsConfig, string name)
        {
            if (Transactions.ContainsKey(kline.Symbol))
            {
                _logger.Log("Not Buying since we already baught it");
            }
            else
            {
                var t = new Transaction(kline.Symbol, kline.Close);
                t.StopLossConfig = GenerateStopLossObject(patternsConfig[name]);
                t.CalculateStopLoss(kline.Close);
                Transactions.Add(t.Symbol, t);
                _logger.Log(String.Format("Trade: Buying {0} at {1}", t.Symbol, t.BuyPrice));
            }
        }

        public StopLossDefinition GenerateStopLossObject(JObject settings)
        {
            var sl = new StopLossDefinition();
            sl.DefaultStopLossThreshold = decimal.Parse(settings["DefaultSLThreshold"].ToString());
            sl.DynamicSLThreshold = decimal.Parse(settings["DynamicSLThreshold"].ToString());
            return sl;
        }

        private bool CheckPatterns(IPattern p, Kline kline )
        {
            var buy = false;
            switch (p.Name)
            {
                case "Spring":
                    {
                        buy = p.CheckPattern(CoinPairDict[kline.Symbol].AvgPrice, kline.CloseTime);
                        break;
                    }
                case "Streak":
                    {
                        buy = p.CheckPattern(kline);
                        break;
                    }
                default:
                    buy = false;
                    break;
            }
            return buy;
        }

        private void SavePriceToQueue(Dictionary<string, CoinPair> coinPairDict, Kline kline)
        {
            var interval = kline.Interval;
            var price = kline.Close;

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

            Patterns = new PatternRepository();
            Factory = new PatternFactory(_logger);

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

        private decimal SetPriceForPattern(IPattern p, Kline kline)
        {
            var priceDef = p.DefinePriceForCalculation(p);
            decimal price = 0;

            switch (priceDef)
            {
                case Pattern.PriceForCalc.AvgClose:
                    {
                        price = CoinPairDict[kline.Symbol].AvgPrice;
                        break;
                    }

                case Pattern.PriceForCalc.Close:
                    {
                        price = kline.Close;
                        break;
                    }

                default:
                    {
                        price = kline.Close;
                        break;
                    }
            }

            return price;
        }
    }
}