using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;
using Crypto.Infra.Rabbit;

namespace Crypto.RuleEngine
{
    partial class RuleEngineService : ServiceBase
    {
        public RuleEngineService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var logger = new Logger("RuleEngine");

            Config.LoadConfiguration(logger);
            var rabbit = new RabbitClient(logger, "BNBClient", Config.RabbitExchanges);
            
            rabbit.Connect();

            logger.Log("*********************************");
            logger.Log("Rule Engine Started Successfully");
            logger.Log("*********************************");
        }

        protected override void OnStop()
        {

        }
    }
}
