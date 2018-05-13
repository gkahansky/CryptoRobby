using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;

namespace Crypto.RuleEngine
{
    class RuleMarketCap : Rule
    {
        public EnumCollection.Condition Condition { get; set; }
        public decimal Value { get; set; }


        public RuleMarketCap(int id,
                            EnumCollection.RuleType type,
                            EnumCollection.Condition condition,
                            decimal value)
            : base(id, type)
        {
            Condition = condition;
            Value = value;
        }
    }
}
