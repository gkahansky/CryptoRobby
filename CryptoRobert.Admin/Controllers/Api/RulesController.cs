using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Data;

namespace CryptoRobert.Admin.Controllers.Api
{
    public class RulesController : ApiController
    {
        private  RuleContext _context;

        //GET api/rules
        public IEnumerable<RuleDefinition> GetRules()
        {
            return _context.RuleDefinitions.ToList();
        }

        //GET api/rules/1
        public RuleDefinition GetRule(int id)
        {
            var rule = _context.RuleDefinitions.FirstOrDefault(r => r.Id == id);

            if (rule == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return rule;
        }

        //POST api/rules
        [HttpPost]
        public RuleDefinition CreateRule(RuleDefinition rule)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            _context.RuleDefinitions.Add(rule);
            _context.SaveChanges();

            return rule;
        }

        //PUT api/rules/1
        public void UpdateRule(int id, RuleDefinition rule)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var ruleInDb = _context.RuleDefinitions.FirstOrDefault(r => r.Id == id);

            if (ruleInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

        }

    }


}
