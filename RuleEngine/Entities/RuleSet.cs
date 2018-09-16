using Crypto.RuleEngine.Interfaces;
using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine.Entities
{
    public class RuleSet
    {
        #region Members
        public int Id;
        public SortedList<int, RuleSetDefinition> Rules;
        public int Score;
        #endregion

        #region CTOR
        public RuleSet(int id)
        {
            Id = id;
            Rules = new SortedList<int, RuleSetDefinition>();
            Score = 0;
        }
        #endregion

        public void Add(RuleSetDefinition def)
        {
            Rules.Add(def.Priority, def);
        }
    }


}
