using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine.Entities
{
    public class RuleDefinitionRepository
    {
        private Dictionary<string, Dictionary<string, RuleDefinition>> Rules { get; set; }

        public RuleDefinitionRepository()
        {
            Rules = new Dictionary<string, Dictionary<string, RuleDefinition>>();
        }



    }
}
