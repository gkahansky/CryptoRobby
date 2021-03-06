﻿using CryptoRobert.Infra;
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


            var repository = new DataRepository();
            repository.Klines = new Queue<Kline>();
            var _rabbit = new RabbitHandler(logger, "BNB");
            var fileHandler = new FileHandler(logger);

            var _dbHandler = new CryptoRobert.Importer.Base.DbHandler(logger);

            MetaDataContainer.KlineQueue = new Queue<List<Kline>>();

            var bnb = new BnbCommunicator(logger, _dbHandler, _rabbit, fileHandler);

            //var gaps = _dbHandler.FindMissingTicks(bnb.metaData.Intervals);

            var dbl = new CryptoRobert.DBLoader.DbHandler(logger,repository);
            repository.Klines.Enqueue(new Kline { Symbol = "GUYBTC", Interval = "5m", OpenTime = 1520249700000, CloseTime = 1520249999999, Open = 0.00002623m, Close = 0.00002630m, High = 0.00002641m, Low = 0.00002620m, Volume = 883552.00000000m });

            //bnb.FillGapsinDb();

            

            //bnb.GetKlinesForDbGaps(gaps);

            bnb.UpdateTickerPrices();
            bnb.SaveCandleStickData();

        }

    }
}
