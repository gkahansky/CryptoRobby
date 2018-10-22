using CryptoRobert.Admin.Models;
using CryptoRobert.RuleEngine;
using CryptoRobert.RuleEngine.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoRobert.Admin.Controllers
{
    public class RuleDefinitionController : GlobalController
    {
        private IDataHandler dbHandler { get; set; }
        public RuleDefinitionsModel Rules { get; set; }

        public RuleDefinitionController()
        {
            dbHandler = new DataHandler(this.logger);
            Rules = new RuleDefinitionsModel();

        }
        // GET: RuleDefinition
        public ActionResult Index()
        {
            var dbRules = dbHandler.LoadRulesFromDb();
            var setsToRules = dbHandler.LoadRuleSetToRulesFromDb();
            Rules.RuleList = ConvertDbRulesToModel(dbRules);
            foreach (var rule in Rules.RuleList)
            {
                rule.SetsAssigned = AssignSetsToRuleDefinition(setsToRules, rule.Id);
            }
            return View(Rules);
        }



        public ActionResult Edit(int id)
        {
            var dbRules = dbHandler.LoadRulesFromDb(id);
            var rule = ConvertDbRulesToModel(dbRules);
            return View(rule[0]);
        }

        public ActionResult Delete(int Id)
        {
            var rule = dbHandler.GetRuleById(Id);
            if (rule != null)
            {
                var ruleModel = ConvertDbRulesToModel(new List<RuleDefinition> { rule });
                return View(ruleModel[0]);
            }
                
            else
                return View("Oops! Rule Not Found");
        }

        public ActionResult New()
        {
            var rule = new RuleDefinitionModel();

            return View(rule);
        }

        [HttpPost]
        public ActionResult DeleteRule(int id)
        {
            dbHandler.DeleteRuleDefinition(id);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Create(RuleDefinitionModel def)
        {
            var rule = new RuleDefinition();
            rule.Id = def.Id;
            rule.RuleType = def.RuleType;
            rule.Interval = def.Interval;
            rule.Symbol = def.Symbol;
            rule.Operator = def.Operator;
            rule.Priority = def.Priority;
            rule.State = false;
            rule.Threshold = def.Threshold;
            rule.Retention = def.Retention;
            rule.Key = rule.GenerateKey();

            var success = dbHandler.SaveRuleDefinition(rule);

            return RedirectToAction("Index", "RuleDefinition");
        }

        #region Private Methods

        private List<int> AssignSetsToRuleDefinition(List<RuleSetDefinition> setsToRules, int id)
        {
            var sets = new List<int>();
            foreach (var item in setsToRules)
            {
                if (item.RuleId == id)
                    sets.Add(item.Id);
            }
            return sets;
        }

        private List<RuleDefinitionModel> ConvertDbRulesToModel(List<RuleDefinition> dbRules)
        {
            var list = new List<RuleDefinitionModel>();
            if (dbRules.Count > 0)
            {
                foreach (var rule in dbRules)
                {
                    var r = new RuleDefinitionModel();
                    r.Id = rule.Id;
                    r.Symbol = rule.Symbol;
                    r.Interval = rule.Interval;
                    r.Operator = rule.Operator;
                    r.Priority = rule.Priority;
                    r.Retention = rule.Retention;
                    r.State = rule.State;
                    r.Threshold = rule.Threshold;
                    r.Key = rule.Key;
                    r.RuleType = rule.RuleType;
                    list.Add(r);
                }
            }
            return list;
        }


        public List<int> GetAllRuleIds()
        {
            var ruleIds = new List<int>();
            var rules = dbHandler.LoadRulesFromDb();
            foreach(var r in rules)
            {
                ruleIds.Add(r.Id);
            }
            return ruleIds;
        }

        #endregion
    }
}