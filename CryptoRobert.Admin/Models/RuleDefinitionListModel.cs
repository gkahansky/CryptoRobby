using CryptoRobert.RuleEngine.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoRobert.Admin.Models
{
    public class RuleDefinitionListModel
    {
        List<RuleDefinition> Rules { get; set; }

        public RuleDefinitionListModel()
        {
            Rules = new List<RuleDefinition>();
        }
    }
}