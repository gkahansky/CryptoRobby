using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.IO;
using System.Threading.Tasks;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Rabbit;
using CryptoRobert.Importer.Base;

namespace CryptoRobert.Importer.Bnb
{
    public partial class ImporterBnb : ServiceBase
    {
        private BnbCommunicator BnbImporter { get; set; }

        public ImporterBnb()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var logger = new Logger("BnbImporter");
            var dbHandler = new DbHandler(logger);
            var rabbit = new RabbitHandler(logger, "BNB");
            Config.LoadConfiguration(logger);
            MetaDataContainer.KlineQueue = new Queue<List<Kline>>();

            BnbImporter = new BnbCommunicator(logger,dbHandler, rabbit);

            //BnbImporter.CoinPairs = new Dictionary<string, CoinPair>();

            //Initialize Binance Client
            var restClient = BnbImporter.Connect();


            float sampleInterval = Config.BinanceSampleInterval / 60000;
            
            BnbImporter.UpdateTickerPrices();
            

            System.Timers.Timer timer = new System.Timers.Timer(Config.BinanceSampleInterval);
            timer.AutoReset = true;
            timer.Enabled = true;

            timer.Elapsed += Timer_Elapsed;
            BnbImporter.SaveCandleStickData();
        }

        
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Config.ReloadConfiguration();
            BnbImporter.UpdateTickerPrices();
            BnbImporter.SaveCandleStickData(); 
            //BnbImporter.UpdateKlines();
        }

        protected override void OnStop()
        {
            BnbImporter.Leave();
        }
    }
}
