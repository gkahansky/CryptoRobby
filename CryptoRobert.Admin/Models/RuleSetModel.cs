using CryptoRobert.RuleEngine.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoRobert.Admin.Models
{
    //[Authorize]
    public class RuleSetModel
    {
        [Required]
        public int      Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string   PairToBuy   { get; set; }
        public decimal  Score            { get; set; }
        public decimal  Threshold        { get; set; }
        public string   LastModified { get; set; }
        public IEnumerable<Pair> pairs { get; set; }
        public List<int> RulesAssigned { get; set; }

        public RuleSetModel()
        {
            pairs = new List<Pair>();
            RulesAssigned = new List<int>();
        }
    }
}