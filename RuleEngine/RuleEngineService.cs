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
using RabbitMQ.Client;

namespace Crypto.RuleEngine
{
    partial class RuleEngineService : ServiceBase
    {
        private RabbitClient Rabbit;
        private String Name;
        private IModel Model;
        public RuleEngineService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var logger = new Logger("RuleEngine");
            Name = "RuleEngine";
            Config.LoadConfiguration(logger);
            Rabbit = new RabbitClient(logger, Name, Config.RabbitExchanges);

            Model = Rabbit.Connect();

            logger.Log("*********************************");
            logger.Log("Rule Engine Started Successfully");
            logger.Log("*********************************");

            Rabbit.InitializeConsumer(Name, Model);

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.AutoReset = true;
            timer.Enabled = true;

            timer.Elapsed += Timer_Elapsed;

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
        }

        protected override void OnStop()
        {
            Rabbit.Dispose();
        }
    }
}
