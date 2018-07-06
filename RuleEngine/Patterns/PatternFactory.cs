using System;
using System.Collections.Generic;
using System.Linq;
using CryptoRobert.Infra;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra.Patterns;

namespace CryptoRobert.RuleEngine
{
    public class PatternFactory 
    {

        private Dictionary<string, Type> patternsTypes;
        protected virtual ILogger _logger { get; set; }

        public PatternFactory(ILogger logger)
        {
            _logger = logger;
            LoadPatternTypesForUser();
        }


        public IPattern CreateInstance(string name)
        {
            Type t = GetTypeToCreate(name);

            if (t == null)
                throw new ArgumentException("Pattern Not Supported");

            return Activator.CreateInstance(t) as IPattern;
        }
        

        private Type GetTypeToCreate(string name)
        {
            foreach(var pattern in patternsTypes)
            {
                if (name.ToLower() == pattern.Key.ToLower())
                    return patternsTypes[pattern.Key];
            }
            return null;
        }

        private void LoadPatternTypesForUser()
        {
            patternsTypes = new Dictionary<string, Type>();

            patternsTypes.Add("Spring", typeof(SpringPattern));
            patternsTypes.Add("Streak", typeof(StreakPattern));
            //foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            //{

            //    if (type.IsSubclassOf(typeof(Pattern)))
            //    {
            //        patternsTypes.Add(type.Name.ToLower(), type);
            //        _logger.Info("New Pattern added to configuration: " + type.Name);
            //    }
            //}
        }

    }
}
