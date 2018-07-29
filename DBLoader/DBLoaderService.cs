using CryptoRobert.Infra;
using CryptoRobert.Infra.Rabbit;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.DBLoader
{
    public partial class DBLoaderService : ServiceBase
    {
        private RabbitClient Rabbit;
        private String Name;
        private IModel Model;
        private DataRepository Repository;
        private ILogger _logger;


        public DBLoaderService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _logger = new Logger("DBLoader");
            
            Config.LoadConfiguration(_logger);
            Repository = new DataRepository();
            Repository.Klines = new Queue<Kline>();

            Name = Config.DbHandlerQueue;
            var dbHandler = new DbHandler(_logger, Repository);

            Rabbit = new RabbitClient(_logger, Name, Config.RabbitExchanges, Repository);

            Rabbit.KlineReceived += dbHandler.OnKlineReceived;

            Model = Rabbit.Connect();
            Rabbit.InitializeConsumer(Name, Model, Repository);

            _logger.Info("*********************************");
            _logger.Info("DBLoader Started Successfully");
            _logger.Info("*********************************");


        }

        private void Dispose()
        {
            this.Model.Dispose();
            this.Rabbit.Dispose();
            _logger.Info("************************");
            _logger.Info("DBLoader Service Stopped");
            _logger.Info("************************");
        }

        protected override void OnStop()
        {
            Dispose();
        }
    }
}
