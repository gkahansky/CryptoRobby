using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;

namespace Crypto.RuleEngine
{
    public class Rule
    {
        public int Id { get; set; }
        public EnumCollection.RuleType Type { get; set; }

        public Rule(int id, EnumCollection.RuleType type)
        {
            Id = id;
            Type = type;
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
