using Crypto.Infra.Rules;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra.Patterns
{
    public class RulePattern : Pattern
    {
        private ILogger logger;
        public List<IRule> Rules {get;set;}

        public RulePattern(ILogger _logger, PatternConfig settings, string engineName = "Generic") 
            : base(settings, _logger, engineName)
        {
            Rules = new List<IRule>();
            Retention = settings.Retention;
            Name = settings.Name;
            Threshold = settings.Threshold;
            logger = _logger;
            DefaultStopLossThreshold = settings.DefaultStopLoss;
            DynamicStopLossThreshold = settings.DynamicStopLoss;
            AddRules();
        }

        private void AddRules()
        {
            
            Rules.Add(new RulePriceTrendUp(Symbol, Interval, Retention, Threshold, 0));
            //Rules.Add(trendRule.Priority, trendRule);
            var orderedList = Rules.OrderBy(rule => rule.Priority);


        }

        public override int CheckPattern(Kline kline)
        {
            var buy = true;
            foreach(var rule in Rules)
            {
                rule.CheckRule(kline);
                if (!rule.IsAchieved)
                    buy = false;
            }

            if (buy)
                return 1;
            else
                return 0;
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
    }
}
