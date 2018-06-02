using Crypto.Infra;
using Crypto.Importer;
using System.Threading;
using System.Configuration;
using System;
using System.Collections.Generic;
using Crypto.Infra.Data;
using Crypto.Importer.Cmc;
using Crypto.Importer.Bnb;
using Crypto.Importer.Base;
using Crypto.RuleEngine;
using M3C.Finance.BinanceSdk;
using M3C.Finance.BinanceSdk.Methods;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;


namespace RobbyConsole
{
    class Program
    {
        private readonly IDbHandler _dbHandler;
        static void Main(string[] args)
        {
             var logger = new Logger("Robby");

            var pFactory = new PatternFactory(logger);

            var p = new PatternSpring(logger);

            decimal[] prices = new decimal[] { 5.4m, 5, 6.1m, 6.11m, 7, 6.998m, 6.9m, 6.5m, 6.55m, 6.6m, 6.7m, 6.9m, 6.997m, 6.99m, 7.1m, 7.5m };
            //decimal[] prices = new decimal[] { 5, 7, 9, 8, 10 };

            foreach(var price in prices)
            {
                p.CheckPattern(price);
            }

            

            //BnbImporter
            // Config.SqlConnectionString = "Data Source=KAHANSKY;Initial Catalog=Crypto;User Id=CryptoAdmin;Password=CryptoAdmin";


            // MetaDataContainer.KlineQueue = new Queue<List<Kline>>();

            // var dbl = new DbHandler(logger);
            //MetaDataContainer.KlineQueue = new Queue<List<Kline>>();
            // MetaData meta = new MetaData();


            // Config.LoadConfiguration(logger);


            // var bnb = new BnbCommunicator(logger, dbl);
            // bnb.UpdateTickerPrices();
            // bnb.SaveCandleStickData();
            // Console.ReadKey();

        }

    }
}
