using CryptoRobert.Infra;
using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.BusinessLogic
{
    public class RuleCalculator : IRuleCalculator
    {
        public Dictionary<int, Operator> Operators { get; set; }
        private ILogger _logger { get; set; }

        public RuleCalculator(ILogger logger)
        {
            _logger = logger;
            SetOperators();

        }

        private void SetOperators()
        {
            Operators = new Dictionary<int, Operator>();
            Operators.Add(0, new Operator { Id = 0, Name = "Equals", DisplayName = "Equals" });
            Operators.Add(1, new Operator { Id = 1, Name = "GreaterThan", DisplayName = "Greater Than" });
            Operators.Add(2, new Operator { Id = 2, Name = "LessThan", DisplayName = "Less Than" });
            Operators.Add(3, new Operator { Id = 3, Name = "GreaterOrEqual", DisplayName = "Greater Than Or Equals" });
            Operators.Add(4, new Operator { Id = 4, Name = "LessOrEqual", DisplayName = "Less Than Or Equals" });
        }

        public decimal CalculateTrend(decimal LastAvgPrice, decimal avgPrice)
        {
            decimal avgPriceChange = 0;
            if (LastAvgPrice > 0)
            {
                var avgPriceDelta = avgPrice - LastAvgPrice;
                avgPriceChange = (avgPriceDelta / avgPrice);
            }
            return avgPriceChange;
        }

        public bool CheckThreshold(decimal threshold, decimal value, int op)
        {
            var isTrue = false ;
            switch (op)
            {
                case 0:
                    return isTrue = (threshold == value);

                case 1:
                    return isTrue = (threshold > value);

                case 2:
                    return isTrue = (threshold < value);

                case 3:
                    return isTrue = (threshold >= value);

                case 4:
                    return isTrue = (threshold <= value);

                default:
                    return false;
            }
        }
    }
}
