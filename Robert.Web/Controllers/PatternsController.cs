using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CryptoRobert.Infra;
using CryptoRobert.RuleEngine.Patterns;
using CryptoRobert.Web.Models;

namespace CryptoRobert.Web.Controllers
{
    public class PatternsController : Controller
    {
        // GET: Patterns
        public ActionResult Random()
        {
            var logger = new Logger("RobertWeb");
            var conf = new PatternConfig("Test", "ETHBTC", "15m");

            var pattern = new PatternModel();
            pattern.Name = "TestModel";
            pattern.Symbol = "BTCUSD";
            pattern.Interval = "1d";
            return View(pattern);
        }
    }
}