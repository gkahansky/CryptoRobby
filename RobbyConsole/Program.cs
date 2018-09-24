using CryptoRobert.Infra;
using CryptoRobert.Importer;
using System.Threading;
using System.Configuration;
using System;
using System.Collections.Generic;
using CryptoRobert.Infra.Data;
using CryptoRobert.Infra.Rabbit;
using CryptoRobert.Importer.Cmc;
using CryptoRobert.Importer.Bnb;
using CryptoRobert.Importer.Base;
using CryptoRobert.RuleEngine;
using M3C.Finance.BinanceSdk;
using M3C.Finance.BinanceSdk.Methods;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using RabbitMQ.Client;
using CryptoRobert.RuleEngine.Patterns;
using CryptoRobert.Infra.Patterns;
using CryptoRobert.RuleEngine.Entities;
using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.RuleEngine.Data;
using CryptoRobert.RuleEngine.Entities.Repositories;
using CryptoRobert.RuleEngine.BusinessLogic;

namespace RobbyConsole
{
    class Program
    {
        private static CryptoRobert.Importer.Base.IDbHandler _dbHandler;
        private readonly IDataHandler dataHandler;


        static void Main(string[] args)
        {


            var logger = new Logger("Robby");
            Config.LoadConfiguration(logger);

            IRuleRepository repo = new RuleRepository(logger);
            IRuleDefinitionRepository defRepo = new RuleDefinitionRepository(logger);
            IRuleSetRepository setRepo = new RuleSetRepository(logger);
            IRuleCalculator calc = new RuleCalculator(logger);
            IDataHandler handler = new DataHandler(logger);
            DataRepository dataRepo = new DataRepository();



            var manager = new RuleManager(logger, repo, defRepo, setRepo, handler, calc);
            var validator = new RuleValidator(logger, repo, defRepo, setRepo,calc, dataRepo);

            manager.RuleConfigurationInitialize();

            var k1 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527811200000, CloseTime = 1527811499999, Open = 0.00163390m, Close = 0.01162960m, High = 0.00163390m, Low = 0.00162820m, Volume = 38760.8800000m };
            var k2 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527811500000, CloseTime = 1527811799999, Open = 0.00162960m, Close = 0.02162910m, High = 0.00163460m, Low = 0.00162630m, Volume = 38335.7000000m };
            var k3 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527811800000, CloseTime = 1527812099999, Open = 0.00162910m, Close = 0.03162890m, High = 0.00163260m, Low = 0.00162660m, Volume = 44551.5500000m };
            var k4 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527812100000, CloseTime = 1527812399999, Open = 0.00162910m, Close = 0.04162920m, High = 0.00163170m, Low = 0.00162820m, Volume = 27189.2000000m };
            var k5 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527812400000, CloseTime = 1527812699999, Open = 0.00162930m, Close = 0.05163210m, High = 0.00163400m, Low = 0.00162720m, Volume = 41544.6300000m };
            var k6 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527812700000, CloseTime = 1527812999999, Open = 0.00163170m, Close = 0.05163000m, High = 0.00163410m, Low = 0.00162800m, Volume = 31390.5200000m };
            var k7 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527813000000, CloseTime = 1527813299999, Open = 0.00163180m, Close = 0.06162950m, High = 0.00163280m, Low = 0.00162950m, Volume = 39613.3100000m };
            var k8 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527813300000, CloseTime = 1527813599999, Open = 0.00162960m, Close = 0.06163250m, High = 0.00163280m, Low = 0.00162820m, Volume = 36159.3100000m };
            var k9 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527813600000, CloseTime = 1527813899999, Open = 0.00163250m, Close = 0.06163370m, High = 0.00163420m, Low = 0.00163180m, Volume = 45087.0700000m };
            var k10 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527813900000, CloseTime = 1527814199999, Open = 0.00163380m, Close = 0.07163290m, High = 0.00163420m, Low = 0.00163080m, Volume = 35669.8000000m };
            var k11 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527814200000, CloseTime = 1527814499999, Open = 0.00163290m, Close = 0.08163390m, High = 0.00163530m, Low = 0.00163110m, Volume = 25887.0500000m };
            var k12 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527814500000, CloseTime = 1527814799999, Open = 0.00163230m, Close = 0.08163090m, High = 0.00163370m, Low = 0.00163000m, Volume = 18803.4800000m };
            var k13 = new Kline { Symbol = "ETHBTC", Interval = "5m", OpenTime = 1527814800000, CloseTime = 1527815099999, Open = 0.00163090m, Close = 0.09163110m, High = 0.00163320m, Low = 0.00162880m, Volume = 24896.6000000m };

            validator.ProcessKline(k1);
            validator.ProcessKline(k2);
            validator.ProcessKline(k3);
            validator.ProcessKline(k4);
            validator.ProcessKline(k5);
            validator.ProcessKline(k6);
            validator.ProcessKline(k7);
            validator.ProcessKline(k8);
            validator.ProcessKline(k9);
            validator.ProcessKline(k10);


            //*********IMPORTER TESTS**************
            //var repository = new DataRepository();
            //repository.Klines = new Queue<Kline>();
            //var _rabbit = new RabbitHandler(logger, "BNB");
            //var fileHandler = new FileHandler(logger);

            //var _dbHandler = new CryptoRobert.Importer.Base.DbHandler(logger);

            //MetaDataContainer.KlineQueue = new Queue<List<Kline>>();

            //var bnb = new BnbCommunicator(logger, _dbHandler, _rabbit, fileHandler);

            ////var gaps = _dbHandler.FindMissingTicks(bnb.metaData.Intervals);

            //var dbl = new CryptoRobert.DBLoader.DbHandler(logger,repository);
            //repository.Klines.Enqueue(new Kline { Symbol = "GUYBTC", Interval = "5m", OpenTime = 1520249700000, CloseTime = 1520249999999, Open = 0.00002623m, Close = 0.00002630m, High = 0.00002641m, Low = 0.00002620m, Volume = 883552.00000000m });

            ////bnb.FillGapsinDb();



            ////bnb.GetKlinesForDbGaps(gaps);

            //bnb.UpdateTickerPrices();
            //bnb.SaveCandleStickData();
            Console.ReadKey();
        }

    }
}
