using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoRobert.Web.Models
{
    public class PatternModel
    {
        public String Name { get; set; }
        public String Symbol { get; set; }
        public String Interval { get; set; }
        public bool IsActive { get; set; }
        public decimal Threshold { get; set; }
        public decimal DefaultStopLoss { get; set; }
        public decimal DynamicStopLoss { get; set; }
    }
}