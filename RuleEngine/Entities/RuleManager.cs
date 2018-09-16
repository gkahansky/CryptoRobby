using Crypto.RuleEngine.Interfaces;
using CryptoRobert.RuleEngine;
using CryptoRobert.RuleEngine.Entities;
using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RuleEngine.Entities
{
    public class RuleManager
    {
        IRuleRepository ruleRepo { get; set; }
        IRuleSetRepository ruleSetRepo { get; set; }
        IDataHandler dataHandler { get; set; }

        public RuleManager(IRuleRepository repo, IDataHandler handler, IRuleSetRepository setRepo)
        {
            ruleRepo = repo;
            dataHandler = handler;
            ruleSetRepo = setRepo;
        }

        public void RuleConfigurationInitialize()
        {
            //Load Rule Definitions & Generate/Update rules
            var ruleDefinitions = dataHandler.LoadRulesFromDb();
            UpdateRuleRepository(ruleDefinitions);


            //Load Rule Sets
            var ruleSets = dataHandler.LoadRuleSetsFromDb();
            UpdateRuleSetRepository(ruleSets);

        }

        private void UpdateRuleRepository(List<RuleDefinition> ruleDefinitions)
        {
            if (ruleDefinitions.Count() > 0)
            {
                foreach(var def in ruleDefinitions)
                {
                    var rule = GenerateRule(def);
                    ruleRepo.Add(rule);
                }
            }
        }

        private void UpdateRuleSetRepository(List<RuleSetDefinition> defs)
        {
            if (defs.Count() > 0)
            {
                foreach(var def in defs)
                {
                    //Create set if none exists
                    var set = ruleSetRepo.Find(def.Id, def.RuleId);
                    if(set == null)
                    {
                        var newSet = new RuleSet(def.Id);
                        newSet.Rules.Add(def.Id, def);
                    }
                    else
                    {
                        var isNew = true;
                        foreach(var setDef in set.Rules)
                        {
                            if (setDef.Value.Id == def.Id && setDef.Value.RuleId == def.RuleId)
                                isNew = false;
                        }
                        if (isNew)
                            set.Rules.Add(def.Id, def);
                    }
                }
            }
        }

        private IRule GenerateRule(RuleDefinition def)
        {
            switch (def.RuleType)
            {
                case "RulePriceTrend":
                    IRule rule = new RulePriceTrend(def.Symbol, def.Interval, def.Retention, def.Id);
                    return rule;

                default:
                    return null;
            }
        }
    }
}
