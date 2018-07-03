using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Rabbit;
using CryptoRobert.RuleEngine.Data;
using CryptoRobert.RuleEngine.Patterns;
using RabbitMQ.Client;

namespace CryptoRobert.RuleEngine
{
    partial class RuleEngineService : ServiceBase
    {
        private RabbitClient Rabbit;
        private String Name;
        private IModel Model;
        private DataRepository Repository;
        public RuleEngineService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var logger = new Logger("RuleEngine");
            Name = "RuleEngine";
            Config.LoadConfiguration(logger);
            Repository = new DataRepository();
            Repository.Klines = new Queue<Kline>();
            Rabbit = new RabbitClient(logger, Name, Config.RabbitExchanges, Repository);
            var runner = new PatternRunner(logger, Repository);
            var dbHandler = new DataHandler(logger);
            dbHandler.SavePatterns(runner.PatternRepository);
            Rabbit.KlineReceived += runner.OnKlineReceived;


            Model = Rabbit.Connect();

            logger.Log("*********************************");
            logger.Log("Rule Engine Started Successfully");
            logger.Log("*********************************");

            Rabbit.InitializeConsumer(Name, Model, Repository);

            System.Timers.Timer timer = new System.Timers.Timer(100);
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
