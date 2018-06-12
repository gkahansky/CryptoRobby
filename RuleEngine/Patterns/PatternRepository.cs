using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine.Patterns
{
    public class PatternRepository 
    {
        public Dictionary<string, IPattern> Items { get; set; }

        public PatternRepository()
        {
            Items = new Dictionary<string, IPattern>();
        }

        public void Add(IPattern p)
        {
            var hash = p.Symbol + "_" + p.Interval;
            if (!Items.ContainsKey(hash))
                Items.Add(hash, p);
        }
        
        public void Remove(IPattern p)
        {
            var hash = p.Symbol + "_" + p.Interval;
            if (Items.ContainsKey(hash))
                Items.Remove(hash);
        }

        public IPattern Find(string hash)
        {
            if (Items.ContainsKey(hash))
                return Items[hash];
            else
                return null;
        }


    }
}
