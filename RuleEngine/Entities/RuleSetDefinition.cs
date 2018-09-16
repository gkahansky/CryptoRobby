using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine.Entities
{
    public class RuleSetDefinition
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public int Priority { get; set; }

        public RuleSetDefinition(int id, int ruleId, int priority)
        {
            Id = id;
            RuleId = ruleId;
            Priority = priority;
        }

        public RuleSetDefinition()
        {

        }
    }
}
