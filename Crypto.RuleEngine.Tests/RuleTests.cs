using System;
using System.Collections.Generic;
using CryptoRobert.Infra;
using CryptoRobert.RuleEngine;
using CryptoRobert.RuleEngine.BusinessLogic;
using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Entities.Repositories;
using CryptoRobert.RuleEngine.Entities.Rules;
using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.Trading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Crypto.RuleEngine.Tests
{
    [TestClass]
    public class RuleTests
    {
        public List<Kline> klines { get; set; }
        public RuleValidator validator { get; set; }
            
        public void prep()
        {
            this.validator = ConfigurationPrep();
        }

        [TestMethod]
        public void TrendRuleValidationBuy()
        {
            prep();
            this.klines = TrendRuleBuyDataPrep();
            foreach (var kline in klines)
            {
                validator.ProcessKline(kline);
            }

            var t = this.validator.Trade.Transactions;
            Assert.AreEqual(t.Count, 1);
            Assert.AreEqual(t["ETHBTC"].BuyPrice, 0.05162920m);
            Assert.AreEqual(t["ETHBTC"].HighPrice, 0.12162950M);
        }

        [TestMethod]
        public void TrendRuleValidationSell()
        {
            prep();

            var sl = new StopLossDefinition()
            {
                DefaultStopLossThreshold = 0.05m,
                DynamicSLThreshold = 0.02m,
                DynamicStopLoss = 0.0870485000M,
                DefaultStopLoss = 0.0505966160M
            };

            var transaction = new Transaction("ETHBTC", 0.05162920M) { BuyPrice = 0.05162920M, HighPrice = 0.12162950M, StopLossConfig = sl };
            validator.Trade.Transactions.Clear();
            validator.Trade.Transactions.Add("ETHBTC",transaction);

            this.klines = TrendRuleSellDataPrep();
            foreach (var kline in klines)
            {
                validator.ProcessKline(kline);
            }

            var t = this.validator.Trade.Transactions;
            Assert.AreEqual(t.Count, 0);
        }


        private RuleValidator ConfigurationPrep()
        {
            var logger = new Logger("RuleTests");

            IRuleDefinitionRepository defRepo = new RuleDefinitionRepository(logger);
            IRuleRepository ruleRepo = new RuleRepository(logger);
            IRuleSetRepository setRepo = new RuleSetRepository(logger);
            IRuleCalculator calc = new RuleCalculator(logger);
            PriceRepository prices = new PriceRepository();
            ITradeEngine trade = new TradeEngine(logger, prices);

            var ruleDef = new RuleDefinition("ETHBTC", "5m", 3, "RulePriceTrend", 0.01m, 0, 1);
            defRepo.Add(ruleDef);
                        
            var rule = new RulePriceTrend("ETHBTC", "5m", 3, 1, "RulePriceTrend", calc, prices);
            rule.Id = 1;
            ruleRepo.Add(rule, 1);

            var set = new RuleSet(1);
            set.Add(ruleDef);
            set.StopLoss = new StopLossDefinition() { DefaultStopLossThreshold = 0.02m, DynamicSLThreshold=0.05m };
            set.PairToBuy = "ETHBTC";
            var setDef = new RuleSetDefinition(1, 1);
            setRepo.Add(set);
            

            Config.LoadConfiguration(logger);
            IDataHandler handler = new DataHandler(logger);
            DataRepository dataRepo = new DataRepository();
            
            var manager = new RuleManager(logger, ruleRepo, defRepo, setRepo, handler, calc, prices);
            var validator = new RuleValidator(logger, ruleRepo, defRepo, setRepo, calc, trade, dataRepo);
            return validator;
        }

        private List<Kline> TrendRuleBuyDataPrep()
        {
            var list = new List<Kline>();
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527811200000, CloseTime = 1527811499999, Open = 0.00163390m, Close = 0.01162960m, High = 0.00163390m, Low = 0.00162820m, Volume = 38760.8800000m });
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527811500000, CloseTime = 1527811799999, Open = 0.00162960m, Close = 0.02162910m, High = 0.00163460m, Low = 0.00162630m, Volume = 38335.7000000m });
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527811800000, CloseTime = 1527812099999, Open = 0.00162910m, Close = 0.03162890m, High = 0.00163260m, Low = 0.00162660m, Volume = 44551.5500000m });
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527812100000, CloseTime = 1527812399999, Open = 0.00162910m, Close = 0.05162920m, High = 0.00163170m, Low = 0.00162820m, Volume = 27189.2000000m });
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527812400000, CloseTime = 1527812699999, Open = 0.00162930m, Close = 0.07163210m, High = 0.00163400m, Low = 0.00162720m, Volume = 41544.6300000m });
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527812700000, CloseTime = 1527812999999, Open = 0.00163170m, Close = 0.09163000m, High = 0.00163410m, Low = 0.00162800m, Volume = 31390.5200000m });
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527813000000, CloseTime = 1527813299999, Open = 0.00163180m, Close = 0.12162950m, High = 0.00163280m, Low = 0.00162950m, Volume = 39613.3100000m });

            return list;
        }

        private List<Kline> TrendRuleSellDataPrep()
        {
            var list = new List<Kline>();
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527813300000, CloseTime = 1527813599999, Open = 0.00162960m, Close = 0.06163250m, High = 0.00163280m, Low = 0.00162820m, Volume = 36159.3100000m });
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527813600000, CloseTime = 1527813899999, Open = 0.00163250m, Close = 0.02163370m, High = 0.00163420m, Low = 0.00163180m, Volume = 45087.0700000m });
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527813900000, CloseTime = 1527814199999, Open = 0.00163380m, Close = 0.03163290m, High = 0.00163420m, Low = 0.00163080m, Volume = 35669.8000000m });
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527814200000, CloseTime = 1527814499999, Open = 0.00163290m, Close = 0.02163390m, High = 0.00163530m, Low = 0.00163110m, Volume = 25887.0500000m });
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527814500000, CloseTime = 1527814799999, Open = 0.00163230m, Close = 0.03163090m, High = 0.00163370m, Low = 0.00163000m, Volume = 18803.4800000m });
            list.Add(new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527814800000, CloseTime = 1527815099999, Open = 0.00163090m, Close = 0.02163110m, High = 0.00163320m, Low = 0.00162880m, Volume = 24896.6000000m });

            return list;
        }
    }
}
