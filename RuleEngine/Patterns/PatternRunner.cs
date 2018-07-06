using CryptoRobert.Infra;
using CryptoRobert.Infra.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Patterns
{
    public class PatternRunner
    {
        private ILogger _logger;
        public Dictionary<string, IPattern> PatternRepository;
        private Dictionary<string, CoinPair> CoinPairDict;
        public  DataRepository KlineRepository;
        public TradeEngine Trade;

        public PatternRunner(ILogger logger, DataRepository repository)
        {
            _logger = logger;
            PatternRepository = InitializePatternRepository();
            CoinPairDict = new Dictionary<string, CoinPair>();
            KlineRepository = repository;
            Trade = new TradeEngine(_logger);
        }

        public void OnKlineReceived(object source, EventArgs e)
        {
            var kline = KlineRepository.Klines.Peek();
            RunPatterns(kline);
            KlineRepository.Klines.Dequeue();
        }

        private Dictionary<string, IPattern> InitializePatternRepository()
        {
            try
            {
                var patternRepo = new Dictionary<string, IPattern>();
                var factory = new PatternFactory(_logger);
                if (!Config.TestMode)
                {
                    foreach (var c in Config.PatternsConfig)
                    {
                        var type = c.Key.Substring(0, c.Key.IndexOf('_'));

                        switch (type)
                        {
                            case "Spring":
                                {
                                    patternRepo.Add(c.Key, new SpringPattern(_logger, c.Value));
                                    break;
                                }
                            case "Streak":
                                {
                                    patternRepo.Add(c.Key, new StreakPattern(_logger, c.Value));
                                    break;
                                }
                        }
                    }
                }
                
                return patternRepo;
            }
            catch (Exception e)
            {
                _logger.Info("Failed to initialize Pattern Repository.\n" + e.ToString());
                throw;
            }
        }

        public void RunPatterns(Kline kline)
        {
           
            var partialKey = kline.Symbol + "_" + kline.Interval;
            var partialDict = PartialMatch(PatternRepository, partialKey);
            var sell = false;
            if (partialDict.Count() > 0)
            {
                _logger.Info(string.Format("Checking Patterns for {0} {1}", kline.Symbol, kline.Interval));
                foreach (var p in partialDict)
                {
                    var buy = p.CheckPattern(kline);

                    if (buy)
                        Trade.BuyPair(kline, p, p.Name);

                    else
                               if (Trade.Transactions.Count > 0)
                        sell = Trade.CheckStopLoss(p, kline);

                    if (sell)
                        Trade.Sell(kline.Symbol, kline.Close);
                }
            }
        }

        public IEnumerable<T> PartialMatch<T>(Dictionary<string, T> dictionary, string partialKey)
        {
            // This, or use a RegEx or whatever.
            IEnumerable<string> fullMatchingKeys =
                dictionary.Keys.Where(currentKey => currentKey.Contains(partialKey));

            List<T> returnedValues = new List<T>();

            foreach (string currentKey in fullMatchingKeys)
            {
                returnedValues.Add(dictionary[currentKey]);
            }

            return returnedValues;
        }
    }
}
