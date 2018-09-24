using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.RuleEngine.Entities;
using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Entities.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;

namespace CryptoRobert.RuleEngine.BusinessLogic
{
    public class RuleManager
    {
        #region Members
        IRuleRepository ruleRepo { get; set; }
        IRuleDefinitionRepository ruleDefRepo { get; set; }
        IRuleSetRepository ruleSetRepo { get; set; }
        IDataHandler dataHandler { get; set; }
        IRuleCalculator calculator { get; set; }
        ILogger _logger { get; set; }

        int nextId { get; set; }

        #endregion

        #region CTOR
        public RuleManager(ILogger logger, IRuleRepository repo, IRuleDefinitionRepository defRepo, IRuleSetRepository setRepo, IDataHandler handler, IRuleCalculator calc)
        {
            _logger = logger;
            ruleRepo = repo;
            ruleDefRepo = defRepo;
            dataHandler = handler;
            ruleSetRepo = setRepo;
            nextId = 1;
            calculator = calc;
        }
        #endregion

        #region Public Methods
        public void RuleConfigurationInitialize()
        {
            //Load Rule Definitions & Generate/Update rules
            var ruleDefinitions = dataHandler.LoadRulesFromDb();
            UpdateRuleRepository(ruleDefinitions);

            var sets = dataHandler.LoadRuleSetsFromDb();
            UpdateRuleSetRepository(sets);
            //Load Rule Sets
            var ruleSetDefs = dataHandler.LoadRuleSetToRulesFromDb();
            UpdateRuleSetToRules(ruleSetDefs);

        }

        private void UpdateRuleSetRepository(List<RuleSet> sets)
        {
            foreach (var set in sets)
            {
                if (!ruleSetRepo.RuleSets.ContainsKey(set.Id))
                {
                    ruleSetRepo.Add(set);
                }
            }
        }

        private void UpdateRuleSetToRules(List<RuleSetDefinition> defs)
        {
            if (defs.Count() > 0)
            {
                foreach (var def in defs)
                {
                    var rule = ruleDefRepo.FindById(def.RuleId);
                    if (rule == null)
                    {
                        _logger.Warning(string.Format("Rule With Id {0} does not exist in RuleDefinitionRepository", def.RuleId));
                        return;
                    }

                    //Create set if none exists
                    var set = ruleSetRepo.Find(def.Id);
                    if (set != null)
                    {
                        if (!set.Rules.ContainsKey(rule.Key))
                        {
                            set.Add(rule);
                        }
                    }
                    else
                    {
                        _logger.Error(string.Format("Failed to generate Rule Set Definition. Rule Set with Id {0} not found.\n", def.Id));
                    }
                }
            }
        }

        #endregion

        #region Private Methods
        private void UpdateRuleRepository(List<RuleDefinition> ruleDefinitions)
        {
            if (ruleDefinitions.Count() > 0)
            {
                foreach (var def in ruleDefinitions)
                {
                    def.Key = def.GenerateKey();
                    ruleDefRepo.Add(def);

                    var rule = GenerateRule(def);
                    ruleRepo.Add(rule, def.Id);
                    nextId++;
                }
            }
        }

        private IRule GenerateRule(RuleDefinition def)
        {
            switch (def.RuleType)
            {
                case "RulePriceTrend":
                    IRule rule = new RulePriceTrend(def.Symbol, def.Interval, def.Retention, this.nextId, "RulePriceTrend", this.calculator);
                    return rule;

                default:
                    return null;
            }
        }
        #endregion
    }
}
