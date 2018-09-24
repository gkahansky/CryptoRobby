using CryptoRobert.RuleEngine.Interfaces;
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
        public SortedList<string, RuleDefinition>   Rules { get; set; }
        public decimal                              Score { get; set; }
        public decimal                              Threshold { get; set; }
        public bool Buy { get; set; }
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
        }

        public void Add(RuleDefinition def)
        {
            Rules.Add(def.Key, def);
        }

        public void Calculate()
        {
            var score = 0;
            foreach(var rule in Rules)
            {
                if (rule.Value.State)
                    score += 1;
            }
            Score = score / Rules.Count();
            if (score > Threshold)
                Buy = true;
            else
                Buy=false;
        }

    }


}
