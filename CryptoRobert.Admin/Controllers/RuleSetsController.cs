using CryptoRobert.Admin.Models;
using CryptoRobert.Infra;
using CryptoRobert.RuleEngine;
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
        public RuleSetsController()
        {
            this.sets = new RuleSetDictionaryModel();
            this.sets.Sets = new Dictionary<int, RuleSetModel>();
        }
        public ActionResult Index()
        {
            this.sets = GetRuleSets();

            return View(sets);
        }

        public ActionResult Edit(int id = 0)
        {
            RuleSetModel set = new RuleSetModel();

            if (id > 0)
            {
                var setList = GetRuleSets(id);
                if (setList.Sets.Count() == 1)
                    set = setList.Sets.First().Value;
            }
            

            //logger.Info(string.Format("Rule Set Id {0} Was Edited"));
            return View(set);
        }

        private RuleSetDictionaryModel GetRuleSets(int id = 0)
        {
            var dbHandler = new DataHandler(logger);
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
                sets.Sets.Add(rs.Id, rs);
            }


            return sets;
        }
    }
}
