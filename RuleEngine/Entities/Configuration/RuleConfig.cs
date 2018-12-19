using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Entities.Configuration
{
    public class RuleConfig
    {
        public int Id { get; set; }
        public string Interval { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
        public int Retention { get; set; }
        public int Operator { get; set; }
        public decimal Threshold { get; set; }
    }
}
