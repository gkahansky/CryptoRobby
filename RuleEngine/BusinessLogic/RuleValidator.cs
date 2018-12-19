using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.Trading;

namespace CryptoRobert.RuleEngine.BusinessLogic
{
    public class RuleValidator
    {
        #region Members
        IRuleRepository RuleRepo { get; set; }
        IRuleDefinitionRepository RuleDefRepo { get; set; }
        IRuleSetRepository RuleSetRepo { get; set; }
        IRuleCalculator calculator { get; set; }
        ILogger _logger { get; set; }
        public DataRepository KlineRepository;
        public ITradeEngine Trade { get; set; }

        //public delegate void RuleProcessedEventHandler(object source, EventArgs args);
        //public event RuleProcessedEventHandler RuleProcessed;

        #endregion

        #region CTOR

        public RuleValidator(ILogger logger, IRuleRepository ruleRepo, IRuleDefinitionRepository defRepo, IRuleSetRepository ruleSetRepo, IRuleCalculator calc, ITradeEngine trade, DataRepository repository)
        {
            _logger = logger;
            RuleRepo = ruleRepo;
            RuleDefRepo = defRepo;
            RuleSetRepo = ruleSetRepo;
            KlineRepository = repository;
            Trade = trade;
            calculator = calc;
        }

        #endregion

        #region Public Methods

        public void OnKlineReceived(object source, EventArgs e)
        {

            var kline = KlineRepository.Klines.Peek();
            _logger.Debug(string.Format("Kline Received {0}-{1}: Processing...", kline.Symbol, kline.Interval));
            ProcessKline(kline);
            KlineRepository.Klines.Dequeue();
        }

        #endregion

        #region Private Methods

        public void ProcessKline(Kline kline)
        {
            var rulesProcessed = ProcessRules(kline);
            _logger.Debug(string.Format("Rules Processed for {0}: {1}", kline.Symbol, rulesProcessed.Count));
            ProcessRuleDefinitions(rulesProcessed);
            _logger.Debug(string.Format("Rule Definitions Processed for {0}: {1}", kline.Symbol, rulesProcessed.Count));
            BuySellCheck(rulesProcessed, kline);
        }

        private void BuySellCheck(List<string> rulesProcessed, Kline kline)
        {
            BuyPairs(rulesProcessed, kline);
            SellPairs(rulesProcessed, kline);

        }

        private void SellPairs(List<string> rulesProcessed, Kline kline)
        {
            if(Trade.Transactions.Count() > 0)
            {
                if(Trade.Transactions.ContainsKey(kline.Symbol))
                {
                    var t = Trade.Transactions[kline.Symbol];
                    var sell = false;
                    decimal profit = 0;
                    if (t.Symbol == kline.Symbol)
                    {
                        sell = Trade.CheckStopLoss(kline);
                        if (sell)
                            Trade.Sell(kline, out profit);
                    }
                }
            }
        }

        private void BuyPairs(List<string> rulesProcessed, Kline kline)
        {
            var pairsToBuy = ProcessRuleSets(rulesProcessed);
            if (pairsToBuy.Count > 0)
            {
                _logger.Debug(string.Format("Rule Sets Process for {0}: {1}", kline.Symbol, rulesProcessed.Count));
                var stopLoss = RuleSetRepo.Find(pairsToBuy.First().Key).StopLoss;
                PrintSetsToBuy(pairsToBuy);
                Trade.BuyPair(kline, stopLoss);
            }
            else
                _logger.Debug(string.Format("Rule Sets Process for {0}: {1}", kline.Symbol, rulesProcessed.Count));
        }

        private void PrintSetsToBuy(Dictionary<int, int> sets)
        {
            foreach (var id in sets)
            {
                var set = RuleSetRepo.Find(id.Key);
                _logger.Info(string.Format("Set {0} is now FULFILLED. Buying {1}", id.Key,set.PairToBuy ));
            }
        }

        private Dictionary<int,int> ProcessRuleSets(List<string> rulesProcessed)
        {
            var setsFulfilled = new Dictionary<int, int>();
            foreach (var key in rulesProcessed)
            {
                foreach (var set in RuleSetRepo.RuleSets)
                {
                    if (set.Value.Rules.ContainsKey(key))
                    {
                        set.Value.Calculate();
                        if (set.Value.Buy)
                        {
                            _logger.Info(string.Format("Rule Set {0} Threshold Matched!!! Buy {1}", set.Value.Id, set.Value.PairToBuy));
                            var pair = key.Substring(0, key.IndexOf('_'));
                            AddPairToBuyList(setsFulfilled, set.Value.Id);
                        }

                    }

                }
            }
            return setsFulfilled;
        }

        private void AddPairToBuyList(Dictionary<int, int> sets, int setId)
        {
            if (!sets.ContainsKey(setId))
                sets.Add(setId, setId);
        }

        private void ProcessRuleDefinitions(List<string> rulesProcessed)
        {
            foreach (var key in rulesProcessed)
            {
                var defToCalc = PartialMatch(RuleDefRepo.Rules, key);
                var rule = RuleRepo.FindByKey(key);
                foreach (var def in defToCalc)
                {
                    def.State = calculator.CheckThreshold(def.Threshold, rule.Value, def.Operator);
                    _logger.Debug(string.Format("Rule {0} State: {1}", def.Key, def.State));
                }
            }
        }

        private List<string> ProcessRules(Kline kline)
        {
            var symbol = kline.Symbol;
            var interval = kline.Interval;
            var partialKey = symbol + "_" + interval;
            var rulesProcessed = new List<string>();

            if (RuleRepo.Rules.ContainsKey(symbol))
            {
                var rules = PartialMatch(RuleRepo.Rules[symbol], partialKey);
                if (rules.Count() > 0)
                {
                    foreach (var r in rules)
                    {
                        r.Calculate(kline);
                        r.HighPrice = calculator.UpdateHighPrice(r.HighPrice, kline.Close);
                        rulesProcessed.Add(r.Key);
                        _logger.Debug(string.Format("Rule Processed: {0}, Value: {1}", r.Key, r.Value));
                    }
                }
            }
            return rulesProcessed;
        }

        private IEnumerable<T> PartialMatch<T>(Dictionary<string, T> dictionary, string partialKey)
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

        #endregion
    }
}
