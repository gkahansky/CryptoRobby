using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoRobert.Admin.Controllers
{
    public class GlobalController : Controller
    {
        public ILogger logger { get; set; }

        private void InitializeLogger()
        {
            logger = new Logger("Admin");
            logger.Info("Admin Started Successfully");
        }
        

    }
}