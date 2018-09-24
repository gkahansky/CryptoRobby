using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Entities.MetaData
{
    public class RuleSetDefinition
    {
        public int Id { get; set; }
        public int RuleId { get; set; }

        public RuleSetDefinition(int id, int ruleId)
        {
            Id = id;
            RuleId = ruleId;
        }

        public RuleSetDefinition()
        {

        }
    }
}
