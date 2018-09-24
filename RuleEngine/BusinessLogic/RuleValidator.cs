using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Trading;
using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.RuleEngine.Entities.MetaData;

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
        public TradeEngine Trade { get; set; }

        //public delegate void RuleProcessedEventHandler(object source, EventArgs args);
        //public event RuleProcessedEventHandler RuleProcessed;

        #endregion

        #region CTOR

        public RuleValidator(ILogger logger, IRuleRepository ruleRepo, IRuleDefinitionRepository defRepo, IRuleSetRepository ruleSetRepo, IRuleCalculator calc, DataRepository repository)
        {
            _logger = logger;
            RuleRepo = ruleRepo;
            RuleDefRepo = defRepo;
            RuleSetRepo = ruleSetRepo;
            KlineRepository = repository;
            calculator = calc;
            Trade = new TradeEngine(_logger);
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
            ProcessRuleSets(rulesProcessed);
            _logger.Debug(string.Format("Rule Sets Process for {0}: {1}", kline.Symbol, rulesProcessed.Count));
        }

        private void ProcessRuleSets(List<string> rulesProcessed)
        {
            foreach (var key in rulesProcessed)
            {
                foreach (var set in RuleSetRepo.RuleSets)
                {
                    if (set.Value.Rules.ContainsKey(key))
                        set.Value.Calculate();
                }
            }
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
                        rulesProcessed.Add(r.Key);
                        _logger.Debug(string.Format("Rule Processed: {0}, Value: {1}", r.Key,r.Value));
                    }
                }
            }
            return rulesProcessed;
        }

        //public virtual void OnRuleProcessed(string key)
        //{

        //    if (RuleProcessed != null)
        //    {
        //        RuleProcessed(this, EventArgs.Empty);
        //    }
        //}

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
