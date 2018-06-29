using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.Threading.Tasks;
using System.Threading;
using CryptoRobert.Infra;
using CryptoRobert.Importer.Base;
using CryptoRobert.RuleEngine;
using System.Reflection;

namespace CryptoRobert.Importer.Cmc
{
    public partial class ImporterCmc : ServiceBase
    {
        public int SampleInterval { get; set; }
        private CmcCommunicator Cmc { get; set; }

        public ImporterCmc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var logger = new Logger("CmcImporter");
            var dbHandler = new DbHandler(logger);


            Cmc = new CmcCommunicator(logger, dbHandler);
            Cmc.UpdateCmcData();

            System.Timers.Timer timer = new System.Timers.Timer(Config.CmcSampleInterval);
            timer.AutoReset = true;
            timer.Enabled = true;

            timer.Elapsed += Timer_Elapsed;

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Cmc.UpdateCmcData();
        }

        protected override void OnStop()
        {
            Cmc.Leave();
        }
    }
}
