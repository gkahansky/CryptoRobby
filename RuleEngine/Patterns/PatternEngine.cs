using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Patterns;
using CryptoRobert.RuleEngine.Transactions;
using CryptoRobert.RuleEngine.Patterns;
using Newtonsoft.Json.Linq;

namespace CryptoRobert.RuleEngine
{
    public class PatternEngine
    {
        public Dictionary<string, CoinPair> CoinPairDict { get; set; }
        public Dictionary<string, Transaction> Transactions { get; set; }
        PatternFactory Factory { get; set; }
        public PatternRepository Patterns { get; set; }
        public Dictionary<string, StopLossDefinition> StopLossCollection { get; set; }
        public List<decimal> TradeResults { get; set; }
        ILogger _logger;

        public PatternEngine(ILogger logger)
        {
            _logger = logger;
            TradeResults = new List<decimal>();
            CoinPairDict = new Dictionary<string, CoinPair>();
            Factory = new PatternFactory(_logger);
            Patterns = new PatternRepository(_logger);

            StopLossCollection = new Dictionary<string, StopLossDefinition>();
            Transactions = new Dictionary<string, Transaction>();
        }

        public void CheckAllPatterns(List<Kline> klineList, Dictionary<string, IPattern> patterns, Dictionary<string, PatternConfig> patternsConfig)
        {
            if (klineList.Count > 0)
            {
                var runner = InitializeRunner(_logger, patterns);
                var sold = false;
                var trader = new TradeEngine(_logger);
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
                            var buySell = CheckPatterns(p, kline);
                            p.SetHighPrice(kline.High);

                            if (buySell == 1)
                                trader.BuyPair(kline, p, p.Name);
                            else if (buySell == -1)
                                sell = true;
                            else if (trader.Transactions.Count > 0)
                                sell = trader.CheckStopLoss(p, kline);

                            if (sell && trader.Transactions.Count > 0)
                            {
                                    decimal profit;
                                    trader.Sell(kline.Symbol, kline.Close, out profit);
                                    TradeResults.Add(profit);
                            }

                        }
                    }
                }
                _logger.Info("TOTAL PROFIT OF ALL TRADES: " + TradeResults.Sum() + "%");
            }
            else
                _logger.Info("No Relevant Data found for analysis. Please check your query parameters");
        }

        private object InitializeRunner(ILogger logger, Dictionary<string, IPattern> patterns)
        {
            var repo = new DataRepository();
            var runner = new PatternRunner(_logger, repo);

            return runner;
        }

        private void Sell(string symbol, decimal price)
        {
            var t = Transactions[symbol];
            var profit = ((price / t.BuyPrice) - 1) * 100;
            var profitText = profit.ToString();
            if (profitText.Length > 5)
                profitText = profit.ToString().Substring(0, 5);
            var msg = string.Format("Trade: SELLING {0}!!! Buy Price: {1}, Sell Price: {2}, Profit: {3}%", t.Symbol, t.BuyPrice, price.ToString(), profitText);
            _logger.Email("Sell Notice "+ t.Symbol, msg);
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

        private void BuyPair(Kline kline, Dictionary<string, PatternConfig> patternsConfig, string name)
        {
            if (Transactions.ContainsKey(kline.Symbol))
            {
                _logger.Info("Not Buying since we already baught it");
            }
            else
            {
                var t = new Transaction(kline.Symbol, kline.Close);
                t.StopLossConfig = GenerateStopLossObject(patternsConfig[name]);
                t.CalculateStopLoss(kline.Close);
                Transactions.Add(t.Symbol, t);
                _logger.Info(String.Format("Trade: Buying {0} at {1}", t.Symbol, t.BuyPrice));
            }
        }

        public StopLossDefinition GenerateStopLossObject(PatternConfig settings)
        {
            var sl = new StopLossDefinition();
            sl.DefaultStopLossThreshold = settings.DefaultStopLoss;
            sl.DynamicSLThreshold = settings.DynamicStopLoss;
            return sl;
        }

        private int CheckPatterns(IPattern p, Kline kline )
        {
            var buy = 0;
            switch (p.Name)
            {
                case "Spring":
                    {
                        buy = p.CheckPattern(kline);
                        break;
                    }
                case "Streak":
                    {
                        buy = p.CheckPattern(kline);
                        break;
                    }
                case "TrendShift":
                    {
                        buy = p.CheckPattern(kline);
                        break;
                    }
                case "TrendIncline":
                    {
                        buy = p.CheckPattern(kline);
                        break;
                    }
                    
                default:
                    buy = 0;
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

            Patterns = new PatternRepository(_logger);
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
