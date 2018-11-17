using CryptoRobert.RuleEngine.Interfaces;
using System.Data;

namespace CryptoRobert.RuleEngine.Entities.MetaData
{
    public class RuleDefinition
    {
        public int      Id          { get; set; }
        public string   Symbol      { get; set; }
        public string   Interval    { get; set; }
        public int      Retention   { get; set; }
        public string   RuleType    { get; set; }
        public decimal  Threshold   { get; set; }
        public string   Key         { get; set; }
        public int      Operator    { get; set; }       
        public int      Priority    { get; set; }
        public bool     State { get; set; }
        public int      RuleSetId { get; set; }

        public RuleDefinition(string symbol, string interval, int retention, string ruleType, decimal threshold, int priority, int operatorInt, int ruleSetId=0)
        {
            
            Symbol = symbol;
            Interval = interval;
            Retention = retention;
            RuleType = ruleType;
            Threshold = threshold;
            Priority = priority;
            Operator=operatorInt;
            State = false;
            Key = GenerateKey();
            RuleSetId = ruleSetId;
        }

        public RuleDefinition()
        {

        }

        public string GenerateKey()
        {
            string[] attributes = { this.Symbol, this.Interval, this.Retention.ToString(), this.RuleType, this.Id.ToString() };
            string key = string.Join("_", attributes);
            return key;
        }
    }
}