using CryptoRobert.RuleEngine.Interfaces;
using System.Data;

namespace Crypto.RuleEngine.Entities
{
    public class RuleDefinition
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public int Retention { get; set; }
        public string RuleType { get; set; }
        public decimal Threshold { get; set; }
        public int Priority { get; set; }

        public RuleDefinition(string symbol, string interval, int retention, string ruleType, decimal threshold, int priority)
        {

            Symbol = symbol;
            Interval = interval;
            Retention = retention;
            RuleType = ruleType;
            Threshold = threshold;
            Priority = priority;
        }

        public RuleDefinition()
        {

        }
    }
}