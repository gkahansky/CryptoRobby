﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Crypto.Infra;
using Crypto.RuleEngine;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace RuleTester
{

    public partial class CryptoRuleTester : Form
    {
        private ILogger _logger { get; set; }
        private JObject RunSettings { get; set; }

        public CryptoRuleTester()
        {
            InitializeComponent();
            _logger = new Logger("CryptoTesterLog");
            PopulatePatternCombo();
            Config.LoadConfiguration(_logger);
            RunSettings = GenerateDefaultSettings();

        }

        private JObject GenerateDefaultSettings()
        {
            var settings = new JObject();
            retentionText.Text = Config.PatternSpringToKeep.ToString();
            thresholdText.Text = Config.PatternSpringThreshold.ToString();
            symbolText.Text = null;
            intervalText.Text = null;


            return settings;
        }

        private void BrowseButton_OnClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                filePathTextBox.Text = dialog.FileName;
            }

        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            _logger = new Logger("CryptoTesterLog");
            if (!string.IsNullOrEmpty(patternCombo.Text))
            {
                PopulateOutputObject();
                LogConfiguration(RunSettings);
                var executor = new Executor();
                executor.RunTest(_logger, RunSettings);
            }
            else
                MessageBox.Show("Please Select a Pattern");
            
        }

        private void LogConfiguration(JObject runSettings)
        {
            var symbol = "All Pairs";
            var interval = "All Intervals";
            var retention = RunSettings["Retention"].ToString();
            var threshold = RunSettings["Threshold"].ToString();
            var file = RunSettings["Path"].ToString();
            var defaultSl = RunSettings["DefaultSLThreshold"].ToString();
            var DynamicSl = RunSettings["DynamicSLThreshold"].ToString();
            if (!string.IsNullOrWhiteSpace(RunSettings["Symbol"].ToString()))
                symbol = RunSettings["Symbol"].ToString();
            
            if (!string.IsNullOrWhiteSpace(RunSettings["Interval"].ToString()))
                interval = RunSettings["Interval"].ToString();



            var msg = String.Format("Running analysis for {0}_{1} for {2}, MA Aggregation: {3}, Accuracy: {4}, Default Stop Loss Threshold: {5}, Dynamic Stop Loss Threshold: {6}", symbol, interval, file, retention, threshold, defaultSl, DynamicSl);
            _logger.Log(msg);
        }

        private void PopulateOutputObject()
        {
            RunSettings["Retention"] = retentionText.Text;
            RunSettings["Threshold"] = thresholdText.Text;
            if (!string.IsNullOrWhiteSpace(symbolText.Text))
                RunSettings["Symbol"] = symbolText.Text;
            else
                RunSettings["Symbol"] = null;
            if (!string.IsNullOrWhiteSpace(intervalText.Text))
                RunSettings["Interval"] = intervalText.Text;
            else
                RunSettings["Interval"] = null;
            RunSettings["Path"] = filePathTextBox.Text;

            if (!string.IsNullOrWhiteSpace(DefaultSLText.Text))
                RunSettings["DefaultSLThreshold"] = DefaultSLText.Text;
            else
                RunSettings["DefaultSLThreshold"] = 0;

            if (!string.IsNullOrWhiteSpace(DynamicSLText.Text))
                RunSettings["DynamicSLThreshold"] = DynamicSLText.Text;
            else
                RunSettings["DynamicSLThreshold"] = 0;

            RunSettings["Name"] = patternCombo.Text;
        }

        private void PopulatePatternCombo()
        {
            patternCombo.Items.Add("Spring");
            patternCombo.Items.Add("Streak");
        }
    }
}
