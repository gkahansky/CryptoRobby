using CryptoRobert.RuleEngine.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoRobert.Admin.Models
{
    public enum Operator { Equals, GreaterThan, LessThan, GreaterOrEqual, LessOrEqual }

    public class RuleDefinitionModel
    {
        public RuleDefinition RuleDef { get; set; }
        public IEnumerable<RuleSet> RuleSets { get; set; }
        public List<string> Intervals { get; set; }
        public List<string> Pairs { get; set; }
        public List<string> RuleTypes { get; set; }
        public bool Remove { get; set; }

        public RuleDefinitionModel(List<string> intervals, List<string> pairs, List<string> ruleTypes)
        {
            Intervals = intervals;
            Pairs = pairs;
            RuleTypes = ruleTypes;
            Remove = false;
        }

        public RuleDefinitionModel()
        {

        }
    }
}