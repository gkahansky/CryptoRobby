using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;

namespace Crypto.RuleEngine
{
    class RuleCoinPairRates : Rule
    {
        public EnumCollection.Condition Condition { get; set; }
        public decimal Value { get; set; }
        public string CoinPair { get; set; }
        public EnumCollection.ValueType ValueType { get; set; }


        public RuleCoinPairRates(int id,
                            EnumCollection.RuleType type,
                            EnumCollection.Condition condition,
                            decimal value,
                            string coinPair,
                            EnumCollection.ValueType valueType)
            : base(id, type)
        {
            Condition = condition;
            Value = value;
            CoinPair = coinPair;
            ValueType = valueType;
        }
    }
}
