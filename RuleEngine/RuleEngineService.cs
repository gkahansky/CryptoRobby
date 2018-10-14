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
using CryptoRobert.RuleEngine.BusinessLogic;
using CryptoRobert.RuleEngine.Data;
using CryptoRobert.RuleEngine.Entities.Repositories;
using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.Trading;
using RabbitMQ.Client;

namespace CryptoRobert.RuleEngine
{
    partial class RuleEngineService : ServiceBase
    {
        private RabbitClient Rabbit;
        private String Name;
        private IModel Model;
        private DataRepository DataRepo;
        private RuleRepository RuleRepo;
        private RuleDefinitionRepository RuleDefinitionRepo;
        private RuleSetRepository RuleSetRepo;
        private PriceRepository Prices;
        private TradeEngine Trader { get; set; }
        private int counter { get; set; }
        private IRuleCalculator Calculator;
        private RuleManager Manager;
        
        public RuleEngineService()
        {
            InitializeComponent();
            counter = 0;
        }

        protected override void OnStart(string[] args)
        {
            var logger = new Logger("RuleEngine");
            Name = "RuleEngine";
            Config.LoadConfiguration(logger);
            DataRepo = new DataRepository();
            RuleRepo = new RuleRepository(logger);
            RuleDefinitionRepo = new RuleDefinitionRepository(logger);
            RuleSetRepo = new RuleSetRepository(logger);
            var dbHandler = new DataHandler(logger);
            Calculator = new RuleCalculator(logger);
            Prices = new PriceRepository();
            Trader = new TradeEngine(logger, Prices);
            Manager = new RuleManager(logger, RuleRepo, RuleDefinitionRepo, RuleSetRepo, dbHandler, Calculator, Prices); 
            RuleValidator validator = new RuleValidator(logger, RuleRepo, RuleDefinitionRepo, RuleSetRepo, Calculator,Trader, DataRepo);

            Manager.RuleConfigurationInitialize();

            DataRepo.Klines = new Queue<Kline>();
            Rabbit = new RabbitClient(logger, Name, Config.RabbitExchanges, DataRepo);


            //var runner = new PatternRunner(logger, Repository);

            //if (Config.UseSql)
            //    dbHandler.SavePatterns(runner.PatternRepository);
            Rabbit.KlineReceived += validator.OnKlineReceived;


            Model = Rabbit.Connect();

            Rabbit.InitializeConsumer(Name, Model, DataRepo);

            logger.Info("*********************************");
            logger.Info("Rule Engine Started Successfully");
            logger.Info("*********************************");

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
