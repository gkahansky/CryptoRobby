using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.Trading;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Entities.MetaData
{
    public class RuleSet
    {
        #region Members
        public int Id { get; set; }
        public SortedList<string, RuleDefinition> Rules { get; set; }
        public decimal Score { get; set; }
        public decimal Threshold { get; set; }
        public bool Buy { get; set; }
        public string PairToBuy { get; set; }
        public StopLossDefinition StopLoss {get;set;}
        #endregion

        #region CTOR
        public RuleSet(int id)
        {
            Id = id;
            Rules = new SortedList<string, RuleDefinition>();
            Score = 0;
            Threshold = 1;
            Buy = false;
        }
        #endregion

        public RuleSet()
        {
            Rules = new SortedList<string, RuleDefinition>();
            StopLoss = new StopLossDefinition() { DefaultStopLossThreshold = 0.05m, DynamicSLThreshold = 0.02m };
        }

        public void Add(RuleDefinition def)
        {
            var partialKey = def.Key.Substring(0, def.Key.LastIndexOf('_'));
            Rules.Add(partialKey, def);
        }

        public void Calculate()
        {
            decimal score = 0;
            foreach(var rule in Rules)
            {
                if (rule.Value.State)
                    score += 1;
            }
            Score = score / Rules.Count();
            if (Score >= Threshold)
                Buy = true;
            else
                Buy=false;
        }

    }


}
