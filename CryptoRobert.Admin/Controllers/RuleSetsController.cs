using CryptoRobert.Admin.Models;
using CryptoRobert.Infra;
using CryptoRobert.RuleEngine;
using CryptoRobert.RuleEngine.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoRobert.Admin.Controllers
{
    public class RuleSetsController : GlobalController
    {
        // GET: RuleSets
        private RuleSetDictionaryModel sets { get; set; }
        private List<Pair> pairs { get; set; }
        private IDataHandler dbHandler { get; set; }

        public RuleSetsController()
        {
            this.sets = new RuleSetDictionaryModel();
            this.sets.Sets = new Dictionary<int, RuleSetModel>();
            this.pairs = new List<Pair>();
            this.dbHandler = new DataHandler(this.logger);
        }
        public ActionResult Index()
        {
            this.sets = GetRuleSetsModel();

            return View(sets);
        }

        public ActionResult Edit(int id = 0)
        {
            RuleSetModel set = new RuleSetModel();

            if (id > 0)
            {
                var setList = GetRuleSetsModel(id);
                if (setList.Sets.Count() == 1)
                    set = setList.Sets.First().Value;
            }


            //logger.Info(string.Format("Rule Set Id {0} Was Edited"));
            return View(set);
        }

        public ActionResult New()
        {
            var set = new RuleSetModel();
            set.pairs = GetCoinPairs();

            return View(set);
        }


        public ActionResult Delete(int Id)
        {

            List<RuleSet> sets = GetRuleSets(Id);
            var model = GetRuleSetsModel(Id);
            if (sets.Count == 1 && model.Sets.Count == 1)
                return View(model.Sets[Id]);
            else
                return View("Oops! Rule Set Not Found");
        }

        public ActionResult Details(int id)
        {
            var setConfig = dbHandler.LoadRuleSetToRulesFromDb(id);
            var rules = new RuleDefinitionsModel();
            foreach (var rule in setConfig)
            {
                var r = dbHandler.LoadRulesFromDb(rule.Id);
                var rd = ConvertRuleDefToRuleDefModel(r[0]);
                rules.RuleList.Add(rd);
            }
                return View(rules);
        }

        
        [HttpPost]
        public ActionResult DeleteSet(int id)
        {
            dbHandler.DeleteRuleSet(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(RuleSetModel set)
        {
            var ruleSet = new RuleSet();
            ruleSet.Name = set.Name;
            ruleSet.Description = set.Description;
            ruleSet.PairToBuy = set.PairToBuy;
            ruleSet.Threshold = set.Threshold;
            ruleSet.LastModified = DateTime.Now;
            ruleSet.Id = set.Id;
            ruleSet.Score = set.Score;

            var success = dbHandler.SaveRuleSet(ruleSet);

            return RedirectToAction("Index", "RuleSets");
        }

        private List<RuleSet> GetRuleSets(int id = 0)
        {
            var list = new List<RuleSet>();
            list = dbHandler.LoadRuleSetsFromDb(id).ToList();

            return list;
        }

        private List<Pair> GetCoinPairs()
        {
            var pairList = dbHandler.LoadCoinPairsFromDb();
            return pairList;
        }

        private RuleSetDictionaryModel GetRuleSetsModel(int id = 0)
        {
            var list = dbHandler.LoadRuleSetsFromDb(id);
            sets.Sets = new Dictionary<int, RuleSetModel>();

            foreach (var set in list)
            {
                var rs = new RuleSetModel();
                rs.Id = set.Id;
                rs.Name = set.Name;
                rs.Description = set.Description;
                rs.PairToBuy = set.PairToBuy;
                rs.Score = set.Score;
                rs.Threshold = set.Threshold;
                rs.LastModified = set.LastModified.ToString("yyyy-MM-dd hh:mm:ss");
                var rules = dbHandler.LoadRuleSetToRulesFromDb(set.Id);
                foreach(var r in rules)
                {
                    rs.RulesAssigned.Add(r.RuleId);
                }

                sets.Sets.Add(rs.Id, rs);
            }

            return sets;
        }

        private RuleDefinitionModel ConvertRuleDefToRuleDefModel(RuleDefinition r)
        {
            var rd = new RuleDefinitionModel();
            rd.Id = r.Id;
            rd.Interval = r.Interval;
            rd.Key = r.Key;
            rd.Operator = r.Operator;
            rd.Priority = r.Priority;
            rd.Retention = r.Retention;
            rd.RuleType = r.RuleType;
            rd.State = r.State;
            rd.Symbol = r.Symbol;
            rd.Threshold = r.Threshold;
            return rd;
        }
    }
}
