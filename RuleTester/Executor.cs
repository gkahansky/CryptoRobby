﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CryptoRobert.Infra;
using CryptoRobert.RuleEngine;
using CryptoRobert.Infra.Patterns;
using CryptoRobert.Infra.Trading;
using Newtonsoft.Json.Linq;
using CryptoRobert.RuleEngine.Patterns;
using System.Threading;

namespace RuleTester
{
    public class Executor
    {
        private Dictionary<string, PatternConfig> PatternConfig { get; set; }
        //private PatternEngine Engine { get; set; }
        private List<Kline> KlineList { get; set; }
        private FileAnalyzer fileAnalyzer { get; set; }
        private Dictionary<string, IPattern> PatternRepository { get; set; }
        private ILogger logger { get; set; }
        public bool Stop { get; set; }


        #region CTOR
        public Executor(ILogger _logger, FileAnalyzer _fileAnalyzer, Dictionary<string, IPattern> patternRepository)
        {
            logger = _logger;
            fileAnalyzer = _fileAnalyzer;
            PatternRepository = patternRepository;
            KlineList = new List<Kline>();
        }
        #endregion

        public void RunTest(ILogger logger, Dictionary<string, IPattern> patterns, string path)
        {
            KlineList = fileAnalyzer.GenerateKlinesFromCsv(logger, path);

            DataRepository dataRepository = new DataRepository();
            //InitializePatternConfiguration(settings);

            var runner = new PatternRunner(logger, dataRepository);
            logger.InitializeStatsReport();
            runner.PatternRepository = patterns;

            new Thread(() =>
            {
                int i = 0;
            while (!Stop && i < 1)
                {
                    PublishKlines(KlineList, runner);
                    i++;
                }
            }).Start();



        }


        private void PublishKlines(List<Kline> klineList, PatternRunner runner)
        {
            foreach (var kline in klineList)
            {
                runner.RunMultiplePatterns(kline);
            }

            runner.PublishResults();

            foreach (var pattern in runner.PatternRepository)
            {
                logger.Info(string.Format("TOTAL PROFIT OF ALL TRADES for {0} : {1}%", pattern.Value.Engine.Name, pattern.Value.Engine.TradeResults.Sum()));
            }
            
        }


        private void InitializePatternConfiguration(List<PatternConfig> settingsList) //CHANGE
        {
            //PatternConfig = new Dictionary<string, PatternConfig>();
            //PatternConfiguration(settings);
            //var pat = NewPattern(settings.Name, logger, settings);
            //Config.PatternsConfig = new Dictionary<string, PatternConfig>();
            //var hash = pat.Name + "_" + pat.Symbol + "_" + pat.Interval;
            //Config.PatternsConfig.Add(hash, settings);
        }




        private void PatternConfiguration(PatternConfig settings)
        {
            PatternConfig.Add(settings.Name, settings);
        }

        private void PreparePatternSpringConfig(JObject settings)
        {
            Config.PatternsConfig = new Dictionary<string, PatternConfig>();
            var pConfig = new PatternConfig();
            pConfig.Name = settings["Name"].ToString();
            pConfig.Symbol = settings["Symbol"].ToString();
            pConfig.Interval = settings["Interval"].ToString();
            pConfig.Threshold = decimal.Parse(settings["Threshold"].ToString());
            pConfig.Retention = int.Parse(settings["Retention"].ToString());
            pConfig.DefaultStopLoss = decimal.Parse(settings["DefaultSLThreshold"].ToString());
            pConfig.DynamicStopLoss = decimal.Parse(settings["DynamicSLThreshold"].ToString());
            pConfig.IsActive = true;
            Config.PatternSpringThreshold = decimal.Parse(settings["Threshold"].ToString());
            Config.PatternSpringToKeep = int.Parse(settings["Retention"].ToString());
            var hash = pConfig.Name + "_" + pConfig.Symbol + "_" + pConfig.Interval;
            Config.PatternsConfig.Add(hash, pConfig);
        }
    }
}
