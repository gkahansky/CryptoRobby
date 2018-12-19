using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Entities.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Entities.Configuration
{
    public class RuleSetConfig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PairToBuy { get; set; }
        public List<int> Rules { get; set; }
        public decimal Threshold { get; set; }
        public decimal StopLoss { get; set; }
        public decimal DynamicStopLoss { get; set; }
        public bool IsActive { get; set; }
    }
}
