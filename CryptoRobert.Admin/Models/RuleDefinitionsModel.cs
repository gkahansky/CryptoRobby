using CryptoRobert.RuleEngine.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoRobert.Admin.Models
{
    public class RuleDefinitionsModel
    {
        public List<RuleDefinitionModel> RuleList { get; set; }

        public RuleDefinitionsModel()
        {
            RuleList = new List<RuleDefinitionModel>();
        }
    }
}