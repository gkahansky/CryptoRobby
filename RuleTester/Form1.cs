using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using CryptoRobert.Infra;
using CryptoRobert.RuleEngine;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using RuleTester.Entities;
using CryptoRobert.Infra.Patterns;

namespace RuleTester
{

    public partial class CryptoRuleTester : Form
    {
        private ILogger _logger { get; set; }
        private string Path { get; set; }
        private TesterOutput Output { get; set; }
        private FileAnalyzer fileAnalyzer { get; set; }
        private ISettingsBuilder settingsBuilder { get; set; }
        private Executor executor { get; set; }
        private Dictionary<string, IPattern> patterns { get; set; }

        #region CTOR
        public CryptoRuleTester()
        {
            InitializeComponent();
            _logger = new Logger("CryptoTesterLog");
            fileAnalyzer = new FileAnalyzer(_logger);
            settingsBuilder = new SettingsBuilder(_logger);
            Config.LoadConfiguration(_logger, true);
            Output = new TesterOutput();
            Output = GenerateDefaultSettings(Output);
            patterns = new Dictionary<string, IPattern>();
            executor = new Executor(_logger, fileAnalyzer, patterns);


        }
        #endregion

        #region Default Settings
        private TesterOutput GenerateDefaultSettings(TesterOutput output)
        {
            //Patterns
            PopulatePatternCombo();
            PopulateFields();
            output = GenerateTesterOutput(output);
            return output;
        }

        private void PopulateFields()
        {
            //Retention
            retentionMinText.Text = "5";
            retentionMaxText.Text = "15";
            retentionIncText.Text = "1";
            //Threshold
            thresholdMinText.Text = "0.03";
            thresholdMaxText.Text = "0.03";
            thresholdIncText.Text = "0.005";
            //DefaultST
            DefaultSLMinText.Text = "0.05";
            DefaultSLMaxText.Text = "0.05";
            DefaultSLIncText.Text = "0.005";
            //DynamicST
            DynamicSLMinText.Text = "0.02";
            DynamicSLMaxText.Text = "0.02";
            DynamicSLIncText.Text = "0.005";
        }

        private void PopulatePatternCombo()
        {
            PatternsListBox.Items.Add("TrendIncline");
            PatternsListBox.Items.Add("TrendShift");
            PatternsListBox.Items.Add("Spring");
            PatternsListBox.Items.Add("Streak");
            PatternsListBox.Items.Add("RulePattern");
            
        }

        private TesterOutput GenerateTesterOutput(TesterOutput output)
        {
            try
            {
                //Output = new TesterOutput();
                //Retention
                output.retention.Min = decimal.Parse(retentionMinText.Text);
                output.retention.Max = decimal.Parse(retentionMaxText.Text);
                output.retention.Increment = decimal.Parse(retentionIncText.Text);
                //Threshold
                output.threshold.Min = decimal.Parse(thresholdMinText.Text);
                output.threshold.Max = decimal.Parse(thresholdMaxText.Text);
                output.threshold.Increment = decimal.Parse(thresholdIncText.Text);
                //Default Stop Loss
                output.defaultSLThreshold.Min = decimal.Parse(DefaultSLMinText.Text);
                output.defaultSLThreshold.Max = decimal.Parse(DefaultSLMaxText.Text);
                output.defaultSLThreshold.Increment = decimal.Parse(DefaultSLIncText.Text);
                //Default Stop Loss
                output.dynamicSLThreshold.Min = decimal.Parse(DynamicSLMinText.Text);
                output.dynamicSLThreshold.Max = decimal.Parse(DynamicSLMaxText.Text);
                output.dynamicSLThreshold.Increment = decimal.Parse(DynamicSLIncText.Text);

                return output;
            }
            catch (Exception e)
            {
                MessageBox.Show("One of your settings in Invalid! please try again");
                return output;
            }
        }

        #endregion


        #region UI Validations
        private bool ValidateSettings(TesterOutput output)
        {
            var isValid = true;

            isValid = CheckMinMaxValidityInt(output.retention.Min, output.retention.Max);
            if (isValid)
                isValid = CheckMinMaxValidityDecimal(output.threshold.Min, output.threshold.Max);
            else
            {
                MessageBox.Show("Max value must be higher than Min value. Field: Retention");
                return isValid;
            }

            if (isValid)
                isValid = CheckMinMaxValidityDecimal(output.defaultSLThreshold.Min, output.defaultSLThreshold.Max);
            else
            {
                MessageBox.Show("Max value must be higher than Min value. Field: Threshold");
                return isValid;
            }
            if (isValid)
                isValid = CheckMinMaxValidityDecimal(output.dynamicSLThreshold.Min, output.dynamicSLThreshold.Max);
            else
            {
                MessageBox.Show("Max value must be higher than Min value. Field: Default Stop Loss");
                return isValid;
            }
            if (!isValid)
            {
                MessageBox.Show("Max value must be higher than Min value. Field: Dynamic Stop Loss");
                return isValid;
            }

            return isValid;
        }

        private bool CheckMinMaxValidityDecimal(decimal min, decimal max)
        {
            if (min <= max && min > 0 && max > 0)
                return true;
            else
                return false;
        }

        private bool CheckMinMaxValidityInt(decimal minDec, decimal maxDec)
        {
            try
            {
                var min = int.Parse(minDec.ToString());
                var max = int.Parse(maxDec.ToString());

                if (min <= max && min > 0 && max > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string GetUniqueSymbols(Dictionary<string, string> list)
        {
            string listString = "";
            string stringToReturn = "";
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    listString += item.Key + ",";
                }
            }

            if (listString.EndsWith(","))
                stringToReturn = listString.Remove(listString.LastIndexOf(","));
            else
                stringToReturn = listString;

            return stringToReturn;
        }
        #endregion

        private void BrowseButton_OnClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                filePathTextBox.Text = dialog.FileName;
                var symbolIntervals = fileAnalyzer.AnalyzeFileData(filePathTextBox.Text);
                SymbolList.Text = GetUniqueSymbols(symbolIntervals.Item1);
                IntervalList.Text = GetUniqueSymbols(symbolIntervals.Item2);
                var patterns = PatternsListBox.SelectedItems;
            }
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            _logger = new Logger("PatternTester");
            fileAnalyzer = new FileAnalyzer(_logger);
            settingsBuilder = new SettingsBuilder(_logger);
            Config.LoadConfiguration(_logger, true);

            Output = UpdateOutputObject(Output);
            Path = filePathTextBox.Text;
            var isValid = ValidateSettings(Output);
            patterns = settingsBuilder.GenerateSettings(Output);            
            executor.RunTest(_logger, patterns, Path);

        }

        private TesterOutput UpdateOutputObject(TesterOutput output)
        {
            output.Symbols = SymbolList.Text.Split(',');
            output.Intervals = IntervalList.Text.Split(',');


            output = GenerateTesterOutput(output);

            return output;
        }

        private void LogConfiguration(PatternConfig settings)
        {
            var symbol = "All Pairs";
            var interval = "All Intervals";
            var retention = settings.Retention;
            var threshold = settings.Threshold;
            var file = Path;
            var defaultSl = settings.DefaultStopLoss;
            var DynamicSl = settings.DynamicStopLoss;
            if (!string.IsNullOrWhiteSpace(settings.Symbol))
                symbol = settings.Symbol;

            if (!string.IsNullOrWhiteSpace(settings.Interval))
                interval = settings.Interval;



            var msg = String.Format("Running analysis for {0}_{1} for {2}, MA Aggregation: {3}, Accuracy: {4}, Default Stop Loss Threshold: {5}, Dynamic Stop Loss Threshold: {6}", symbol, interval, file, retention, threshold, defaultSl, DynamicSl);
            _logger.Info(msg);
        }

        private void PatternsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var i = PatternsListBox.SelectedIndex;
            var name = PatternsListBox.SelectedItem.ToString();
            var isChecked = PatternsListBox.GetItemChecked(i);

            PatternsListBox.SetItemChecked(i, !isChecked);
            Output.Patterns[name] = !isChecked;

        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            executor.Stop = true;
        }
    }
}
