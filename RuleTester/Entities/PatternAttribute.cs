using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleTester.Entities
{
    public class PatternAttribute
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Increment { get; set; }
    }
}
