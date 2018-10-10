using CryptoRobert.RuleEngine.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoRobert.Admin.Models
{
    //[Authorize]
    public class RuleSetModel
    {
        public int      Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string   PairToBuy   { get; set; }
        public decimal  Score            { get; set; }
        public decimal  Threshold        { get; set; }
        public string   LastModified { get; set; }
    }
}