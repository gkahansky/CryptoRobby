using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using CryptoRobert.RuleEngine.Entities.Rules;
using Newtonsoft.Json.Linq;
using CryptoRobert.RuleEngine.Entities.Configuration;
using CryptoRobert.RuleEngine.BusinessLogic;
using CryptoRobert.Infra;
using CryptoRobert.Trading;
using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Entities.Repositories;
using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.RuleEngine;

namespace RuleTester
{
    public partial class RuleTester : Form
    {
        private ILogger logger;
        private IRuleDefinitionRepository RuleDefRepo { get; set; }
        private IRuleRepository RuleRepo { get; set; }
        private IRuleSetRepository RuleSetRepo { get; set; }
        private IRuleCalculator Calculator { get; set; }
        private ITradeEngine TradeEngine { get; set; }
        private IDataHandler DataHandler { get; set; }
        private DataRepository DataRepo { get; set; }
        private PriceRepository PriceRepo { get; set;}
        private RuleValidator Validator { get; set; }
        private RuleManager Manager { get; set; }
        private TesterConfig ConfigHandler { get; set; }

        public RuleTester()
        {
            ILogger logger = new Logger("RuleTester");
            logger.Info("RuleTester Initiated Succesfully");
            RuleDefRepo = new RuleDefinitionRepository(logger);
            RuleSetRepo = new RuleSetRepository(logger);
            Calculator = new RuleCalculator(logger);
            RuleRepo = new RuleRepository(logger);
            PriceRepo = new PriceRepository();
            DataRepo = new DataRepository();
            DataHandler = new DataHandler(logger);
            Manager = new RuleManager(logger, RuleRepo, RuleDefRepo, RuleSetRepo, DataHandler, Calculator, PriceRepo);
            TradeEngine = new TradeEngine(logger, PriceRepo);
            ConfigHandler = new TesterConfig(logger, RuleRepo, RuleDefRepo, RuleSetRepo,Manager,Calculator);
            Validator = new RuleValidator(logger, RuleRepo, RuleDefRepo, RuleSetRepo, Calculator, TradeEngine, DataRepo);
            
            InitializeComponent();
        }

        private void LoadConfiguration_Button_Click(object sender, EventArgs e)
        {
            var path = ConfigFile_textBox.Text;
            ConfigHandler.LoadConfiguration(path);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                ConfigFile_textBox.Text = dialog.FileName;
            }
        }


        private void Execute_button_Click(object sender, EventArgs e)
        {
            logger = new Logger("RuleTester");
            var path = ConfigFile_textBox.Text;
            ConfigHandler.LoadConfiguration(path);
            Executor executor = new Executor(logger, Validator);
            executor.Execute(0, DataFile_textBox.Text);
            logger.Info("Test Run Completed");
        }

        private void BrowseData_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                DataFile_textBox.Text = dialog.FileName;
            }
        }
    }
}
