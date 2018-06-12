using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleTester
{
    public static class TesterOutput
    {
        public static int Retention { get; set; }
        public static decimal Threshold { get; set; }
        public static string Symbol { get; set; }
        public static string Interval { get; set; }
        public static string Path { get; set; }
        public static decimal DefaultSLThreshold { get; set; }
        public static decimal DynamicSLThreshold { get; set; }
    }
}