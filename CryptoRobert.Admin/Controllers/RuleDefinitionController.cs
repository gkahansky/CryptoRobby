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
        private RuleDefinitionsModel Rules { get; set; }

        public RuleDefinitionController()
        {
            dbHandler = new DataHandler(this.logger);
            Rules = new RuleDefinitionsModel();

        }
        // GET: RuleDefinition
        public ActionResult Index()
        {
            var dbRules = dbHandler.LoadRulesFromDb();
            Rules.RuleList = ConvertDbRulesToModel(dbRules);
            return View(Rules);
        }

        public ActionResult Edit(int id)
        {
            var dbRules = dbHandler.LoadRulesFromDb(id);
            var rule = ConvertDbRulesToModel(dbRules);
            return View(rule[0]);
        }

        public ActionResult New()
        {
            var rule = new RuleDefinitionModel();

            return View(rule);
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



        private List<RuleDefinitionModel> ConvertDbRulesToModel(List<RuleDefinition> dbRules)
        {
            var list = new List<RuleDefinitionModel>();
            if(dbRules.Count > 0)
            {
                foreach(var rule in dbRules)
                {
                    var r = new RuleDefinitionModel();
                    r.Id = rule.Id;
                    r.Symbol = rule.Symbol;
                    r.Interval = rule.Interval;
                    r.Operator= rule.Operator;
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
    }
}