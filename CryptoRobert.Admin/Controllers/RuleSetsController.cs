using CryptoRobert.Admin.Entities;
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

        private RuleSetDictionaryModel sets { get; set; }
        private List<string> pairs { get; set; }
        private IDataHandler dbHandler { get; set; }
        private Mapper mapper { get; set; }

        public RuleSetsController()
        {
            this.sets = new RuleSetDictionaryModel();
            this.sets.Sets = new Dictionary<int, RuleSetModel>();
            this.pairs = new List<string>();
            this.dbHandler = new DataHandler(this.logger);
            this.mapper = new Mapper(this.logger);

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
            var dbSet = dbHandler.LoadRuleSetsFromDb(id);
            var setModel = new RuleSetModel();
            if (dbSet.Count() == 1)
                setModel = mapper.MapRuleSetToModel(dbSet[0]);

            //var setConfig = dbHandler.LoadRuleSetToRulesFromDb(id);
            //var set = dbHandler.LoadRuleSetsFromDb(id);
            //foreach (var rule in setConfig)
            //{
            //    var r = dbHandler.LoadRulesFromDb(rule.RuleId);
            //    var key = r[0].GenerateKey();

            //    set[0].Rules.Add(key, r[0]);
            //}
            //var model = mapper.MapRuleSetToModel(set[0]);
            //foreach (var r in set[0].Rules)
            //{
            //    model.RulesAssigned.Add(r.Value.Id);
            //}

            return View(setModel);
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

            var ruleSet = mapper.MapModelToRuleSet(set);

            var rules = dbHandler.LoadRuleSetToRulesFromDb(set.Id);

            var success = dbHandler.SaveRuleSet(ruleSet);

            return RedirectToAction("Index", "RuleSets");
        }

        #region Private Methods



        private List<RuleSet> GetRuleSets(int id = 0)
        {
            var list = new List<RuleSet>();
            list = dbHandler.LoadRuleSetsFromDb(id).ToList();

            return list;
        }

        private List<string> GetCoinPairs()
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
                var rs = mapper.MapRuleSetToModel(set);
                //rs.Id = set.Id;
                //rs.IsActive = set.IsActive;
                //rs.Name = set.Name;
                //rs.Description = set.Description;
                //rs.PairToBuy = set.PairToBuy;
                //rs.Score = set.Score;
                //rs.Threshold = set.Threshold;
                //rs.LastModified = set.LastModified.ToString("yyyy-MM-dd hh:mm:ss");
                //var rules = dbHandler.LoadRuleSetToRulesFromDb(set.Id);
                //foreach (var r in rules)
                //{
                //    rs.RulesAssigned.Add(r.RuleId);
                //}

                sets.Sets.Add(rs.Id, rs);
            }

            return sets;
        }

        private RuleDefinitionModel ConvertRuleDefToRuleDefModel(RuleDefinition r)
        {
            var rd = new RuleDefinitionModel(new List<string>(), this.pairs, new List<string>());
            rd.RuleDef = r;
            return rd;
        }

        #endregion
    }
}
