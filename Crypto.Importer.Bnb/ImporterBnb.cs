﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.IO;
using System.Threading.Tasks;
using Crypto.Infra;
using Crypto.Importer.Base;

namespace Crypto.Importer.Bnb
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

            BnbImporter = new BnbCommunicator(logger,dbHandler);

            BnbImporter.CoinPairs = new Dictionary<string, CoinPair>();

            //Initialize Binance Client
            var restClient = BnbImporter.Connect();


            float sampleInterval = Config.BinanceSampleInterval / 60000;

            BnbImporter.UpdateTickerPrices();

            System.Timers.Timer timer = new System.Timers.Timer(Config.BinanceSampleInterval);
            timer.AutoReset = true;
            timer.Enabled = true;

            timer.Elapsed += Timer_Elapsed;


        }


        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            BnbImporter.UpdateTickerPrices();
        }

        protected override void OnStop()
        {
            BnbImporter.Leave();
        }
    }
}
