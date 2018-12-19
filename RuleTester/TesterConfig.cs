using CryptoRobert.Infra;
using CryptoRobert.RuleEngine;
using CryptoRobert.RuleEngine.BusinessLogic;
using CryptoRobert.RuleEngine.Entities.Configuration;
using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Entities.Repositories;
using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.Trading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RuleTester
{
    public class TesterConfig
    {
        ILogger logger;
        IRuleRepository RuleRepo;
        IRuleDefinitionRepository RuleDefRepo;
        IRuleSetRepository RuleSetRepo;
        IRuleCalculator Calculator;
        RuleManager Manager; 


        public TesterConfig(ILogger Logger, IRuleRepository ruleRepo, IRuleDefinitionRepository ruleDefRepo, IRuleSetRepository ruleSetRepo, RuleManager manager, IRuleCalculator calculator)
        {
            logger = Logger;
            RuleRepo = ruleRepo;
            RuleDefRepo = ruleDefRepo;
            RuleSetRepo = ruleSetRepo;
            Manager = manager;
            Calculator = calculator;
            Config.TestMode = true;
            Config.LogSeverity = 1;
        }


        public void LoadConfiguration(string path)
        {
            var configJson = LoadConfigFile(path);
            var config = JObject.Parse(configJson.ToString());
            var ruleDefList = LoadRulesFromConfiguration(config);
            var setList = LoadRuleSetsFromConfiguration(config);
            var setDefList = LoadRuleSetDefsFromConfiguration(config);
            Manager.RuleTesterLoadConfiguration(setList, ruleDefList, setDefList);
        }

        private List<RuleSetDefinition> LoadRuleSetDefsFromConfiguration(JObject config)
        {
            var list = new List<RuleSetDefinition>();

            foreach(var set in config["RuleSets"])
            {
                int id = int.Parse(set["Id"].ToString());

                foreach (var def in set["Rules"])
                {
                    int ruleId = int.Parse(def.ToString());
                    var setDef = new RuleSetDefinition(id, ruleId);
                    list.Add(setDef);
                }
            }

            return list;
        }

        private object LoadConfigFile(string path)
        {
            var configText = File.ReadAllText(path);
            var configJson = JsonConvert.DeserializeObject(configText);
            return configJson;
        }

        

        private List<RuleSet> LoadRuleSetsFromConfiguration(JObject config)
        {
            var setList = new List<RuleSet>();
            var ruleDefList = new List<RuleDefinition>();

            foreach (var setDef in config["RuleSets"])
            {
                var set = GenerateSet(setDef);
                setList.Add(set);
            }
            return setList;
        }

        private RuleSet GenerateSet(JToken setDef)
        {
            try
            {
                var set = new RuleSet();
                var setConfig = JsonConvert.DeserializeObject<RuleSetConfig>(setDef.ToString());
                set.Id = setConfig.Id;
                set.Name = setConfig.Name;
                set.Description = setConfig.Description;
                set.PairToBuy = setConfig.PairToBuy;
                set.Threshold = setConfig.Threshold;
                set.Rules = new SortedList<string, RuleDefinition>();
                set.Buy = false;
                set.IsActive = setConfig.IsActive;
                set.StopLoss = GenerateStopLoss(setConfig.StopLoss, setConfig.DynamicStopLoss);

                return set;

            }
            catch (Exception e)
            {
                logger.Error("Failed to generate Rule Set configuration.\n" + e);
                logger.Info("Bad Configuration Object: " + setDef.ToString());
                return null;
            }
        }

        private StopLossDefinition GenerateStopLoss(decimal stopLoss, decimal dynamicStopLoss)
        {
            var sl = new StopLossDefinition();
            sl.DefaultStopLossThreshold = stopLoss;
            sl.DynamicSLThreshold = dynamicStopLoss;
            return sl;
        }

        private SortedList<string, RuleDefinition> GenerateRulesForSet(List<int> rules)
        {
            var rulesList = new SortedList<string, RuleDefinition>();
            foreach (var rule in rules)
            {
                var ruleDef = RuleDefRepo.FindById(rule);
                rulesList.Add(ruleDef.Key, ruleDef);
            }
            return rulesList;
        }

        private List<RuleDefinition> LoadRulesFromConfiguration(JObject config)
        { 
            var ruleDefList = new List<RuleDefinition>();
            foreach (var rule in config["Rules"])
            {
                var ruleConfig = JsonConvert.DeserializeObject<RuleConfig>(rule.ToString());
                var newRuleDef = GenerateRuleDef(ruleConfig);
                ruleDefList.Add(newRuleDef);
            }

            return ruleDefList;
        }

        private RuleDefinition GenerateRuleDef(RuleConfig r)
        {
            var ruleDef = new RuleDefinition();
            ruleDef.Id = r.Id;
            ruleDef.Interval = r.Interval;
            ruleDef.Symbol = r.Symbol;
            ruleDef.Retention = r.Retention;
            ruleDef.RuleType = r.Type;
            ruleDef.Threshold = r.Threshold;
            ruleDef.Operator = r.Operator;
            ruleDef.Key = ruleDef.GenerateKey();

            return ruleDef;
        }

    }
}
