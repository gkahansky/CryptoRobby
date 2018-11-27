using CryptoRobert.Admin.Models;
using CryptoRobert.Infra;
using CryptoRobert.RuleEngine;
using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoRobert.Admin.Entities
{
    public class Mapper
    {
        private DataHandler dbHandler { get; set; }
        private ILogger logger { get; set; }

        public Mapper(ILogger _logger)
        {
            this.logger = _logger;
            dbHandler = new DataHandler(logger);
                
        }
        public RuleDefinitionModel MapRuleDefToModel(RuleDefinition rule, bool IsEdit=true)
        {
            var model = new RuleDefinitionModel();
            model.RuleDef = rule;
            model.RuleSets = dbHandler.LoadSetsByRuleId(rule.Id);
            model.Pairs = dbHandler.LoadCoinPairsFromDb();
            model.RuleTypes = GetAllEntities();
            model.Intervals = GenerateIntervals();

            return model;
        }

        public RuleSetModel MapRuleSetToModel(RuleSet set, bool isEdit = true)
        {
            var model = new RuleSetModel();

            model.Id = set.Id;
            model.IsActive = set.IsActive;
            model.LastModified = set.LastModified.ToString();
            model.Name = set.Name;
            model.Description = set.Description;
            model.PairToBuy = set.PairToBuy;
            model.Score = set.Score;
            model.Threshold = set.Threshold;
           
            var rules = dbHandler.LoadRulesBySet(set.Id);
            foreach(var rule in rules)
            {
                var ruleModel = new RuleDefinitionModel();
                ruleModel = MapRuleDefToModel(rule);
                model.RulesAssigned.Add(ruleModel);
            }

            return model;

        }

        

        public RuleSet MapModelToRuleSet(RuleSetModel model, bool isEdit = true)
        {
            var last = DateTime.Now;
            if (!isEdit)
                last = DateTime.Parse(model.LastModified);

            var set = new RuleSet();
            set.Id = model.Id;
            set.IsActive = model.IsActive;
            set.LastModified = last;
            set.Name = model.Name;
            set.Description = model.Description;
            set.PairToBuy = model.PairToBuy;
            set.Score = model.Score;
            set.Threshold = model.Threshold;
            //ruleSet.Rules = TBD
            //ruleSet.StopLoss = TBD

            return set;

        }

        public List<string> GetAllEntities()
        {
            var types = new List<string>();
            types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(IRule).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .Select(x => x.Name).ToList();
            return types;
        }

        public List<string> GenerateIntervals()
        {
            var list = new List<string>();
            list.Add("1m");
            list.Add("3m");
            list.Add("5m");
            list.Add("15m");
            list.Add("30m");
            list.Add("1h");
            list.Add("2h");
            list.Add("4h");
            list.Add("1d");
            list.Add("1w");
            return list;
        }
    }
}