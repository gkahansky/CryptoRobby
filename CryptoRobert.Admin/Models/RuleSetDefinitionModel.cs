using CryptoRobert.RuleEngine.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoRobert.Admin.Models
{
    public class RuleSetDefinitionModel
    {
        public Dictionary<string, RuleSetDefinition> Sets;

        public RuleSetDefinitionModel()
        {
            Sets = new Dictionary<string, RuleSetDefinition>();
        }
    }
    
}