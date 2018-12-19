using CryptoRobert.Admin.Entities;
using CryptoRobert.Admin.Models;
using CryptoRobert.Infra;
using CryptoRobert.RuleEngine;
using CryptoRobert.RuleEngine.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
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

        public ActionResult Edit(int id = 0)
        {
            var dbSet = dbHandler.LoadRuleSetsFromDb(id);
            var setModel = new RuleSetModel();
            if (dbSet.Count() == 1)
                setModel = mapper.MapRuleSetToModel(dbSet[0]);

            return View(setModel);

        }

        // POST api/RuleSet/1
        [System.Web.Mvc.HttpPost]
        public void EditRuleSet(int id)
        {
            var dbSet = dbHandler.LoadRuleSetsFromDb(id);
            var setModel = new RuleSetModel();
            if (dbSet.Count() == 1)
            {

            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

        }

        public ActionResult Details(int id)
        {
            var dbSet = dbHandler.LoadRuleSetsFromDb(id);
            var setModel = new RuleSetModel();
            if (dbSet.Count() == 1)
                setModel = mapper.MapRuleSetToModel(dbSet[0]);

            return View(setModel);
        }


        [System.Web.Http.HttpDelete]
        public ActionResult DeleteSet(int id)
        {
            dbHandler.DeleteRuleSet(id);
            return RedirectToAction("Index");
        }

        [System.Web.Http.HttpPost]
        public ActionResult Create(RuleSetModel set)
        {
            var rulesToAdd = new List<int>();
            var rulesToRemove = new List<int>();
            var ruleSet = mapper.MapModelToRuleSet(set);

            var rules = dbHandler.LoadRuleSetToRulesFromDb(set.Id);

            var success = dbHandler.SaveRuleSet(ruleSet);
            if (!success)
            {
                logger.Error("Failed to load rule set " + set.Id);
                return RedirectToAction("Index", "RuleSets");
            }

            if (success & ruleSet.Rules.Count()>0)
            {
                rulesToAdd = GetRulesToAdd(set.RulesAssigned, ruleSet.Rules);
                rulesToRemove = GetRulesToRemove(set.RulesAssigned, ruleSet.Rules);
            }

            if (!success)
            {
                logger.Error("Failed to update rule set " + set.Id);
                return RedirectToAction("Index", "RuleSets");
            }

            UpdateRules(rulesToAdd, rulesToRemove, set.Id);

            return RedirectToAction("Index", "RuleSets");
        }

        private void UpdateRules(List<int> rulesToAdd, List<int> rulesToRemove, int setId)
        {
            if (rulesToAdd.Count() > 0)
            {
                foreach (var id in rulesToAdd)
                {
                    dbHandler.AddRulesToRuleSet(rulesToAdd, setId);
                }
            }

            if (rulesToRemove.Count() > 0)
            {
                foreach (var id in rulesToRemove)
                {
                    dbHandler.RemoveRulesToRuleSet(rulesToRemove, setId);
                }
            }
        }

        private List<int> GetRulesToRemove(List<RuleDefinitionModel> rulesAssigned, SortedList<string, RuleDefinition> rules)
        {
            var modelRules = new List<int>();
            foreach (var rule in rulesAssigned)
            {
                modelRules.Add(rule.RuleDef.Id);
            }

            var dbRules = new List<int>();
            foreach (var rule in rulesAssigned)
            {
                dbRules.Add(rule.RuleDef.Id);
            }


            var rulesToRemove = dbRules.Except(modelRules).ToList();
            return rulesToRemove;
        }

        private List<int> GetRulesToAdd(List<RuleDefinitionModel> rulesAssigned, SortedList<string, RuleDefinition> rules)
        {
            var modelRules = new List<int>();
            foreach(var rule in rulesAssigned)
            {
                modelRules.Add(rule.RuleDef.Id);
            }

            var dbRules = new List<int>();
            foreach (var rule in rulesAssigned)
            {
                dbRules.Add(rule.RuleDef.Id);
            }


            var rulesToAdd = modelRules.Except(dbRules).ToList();
            return rulesToAdd;
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
