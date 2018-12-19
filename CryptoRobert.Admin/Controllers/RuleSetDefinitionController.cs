using CryptoRobert.Admin.Entities;
using CryptoRobert.Admin.Models;
using CryptoRobert.RuleEngine;
using CryptoRobert.RuleEngine.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace CryptoRobert.Admin.Controllers
{
    public class RuleSetDefinitionController : GlobalController
    {
        private IDataHandler dbHandler { get; set; }
        private Mapper mapper { get; set; }
        private Dictionary<string,RuleSetDefinition> Sets { get; set; }

        public RuleSetDefinitionController()
        {
            this.dbHandler = new DataHandler(this.logger);
            this.mapper = new Mapper(this.logger);
            Sets = new Dictionary<string, RuleSetDefinition>();

        }

        public ActionResult Index()
        {
            var sets = dbHandler.LoadRuleSetToRulesFromDb();
            foreach(var set in sets)
            {
                var key = set.Id + "_" + set.RuleId;
                if(Sets.Count>0)
                {
                    if (!Sets.ContainsKey(key))
                        Sets.Add(key, set);
                }
                else
                {
                    Sets.Add(key, set);
                }
            }

            var setModel = mapper.MapModelToRuleSetDefinition(Sets);
            return View(setModel);
        }

        //public ActionResult Create(RuleSetDefinitionModel model)
        //{
        //    var setsRules = dbHandler.LoadRuleSetToRulesFromDb();
        //    dbHandler.SaveRuleSetDefinition()
        //}
    }
}
