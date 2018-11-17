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
        //public int Id { get; set; }
        //public string Symbol { get; set; }
        //public string Interval { get; set; }
        //public int Retention { get; set; }
        //public string RuleType { get; set; }
        //public decimal Threshold { get; set; }
        //public string Key { get; set; }
        //public int Operator { get; set; }
        //public int Priority { get; set; }
        //public bool State { get; set; }
        //public int RuleSetId { get; set; }
        public RuleDefinition RuleDef { get; set; }
        public IEnumerable<RuleSet> RuleSets { get; set; }

        public RuleDefinitionModel()
        {
        }
    }
}