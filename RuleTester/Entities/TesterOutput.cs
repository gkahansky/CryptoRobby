using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleTester.Entities
{
    public class TesterOutput
    {
        public Retention retention { get; set; }
        public Threshold threshold { get; set; }
        public string[] Symbols { get; set; }
        public string[] Intervals { get; set; }
        public Dictionary<string, bool> Patterns { get; set; }
        public string Path { get; set; }
        public DefaultSLThreshold defaultSLThreshold { get; set; }
        public DynamicSLThreshold dynamicSLThreshold { get; set; }

        #region CTOR
        public TesterOutput()
        {
            retention = new Retention();
            threshold = new Threshold();
            defaultSLThreshold = new DefaultSLThreshold();
            dynamicSLThreshold = new DynamicSLThreshold();
            Patterns = new Dictionary<string,bool>();

            Patterns.Add("TrendIncline", false);
            Patterns.Add("TrendShift", false);
            Patterns.Add("Spring", false);
            Patterns.Add("Streak", false);
        }
        #endregion
    }
}