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

namespace RobbyConsole
{
    class Program
    {
        private static IDbHandler _dbHandler;
        private readonly IDataHandler dataHandler;

        static IEnumerable<CoinPair> GetNext(CoinPair pair)
        {
            pair.AvgPrice += 1;
            yield return pair;
        }

        static void Main(string[] args)
        {


            var logger = new Logger("Robby");

 


            //var pair = new CoinPair() { Symbol = "ETHBTC", AvgPrice = 1 };

            //var ethMonitor = new CoinMonitor(logger);
            //var ethTicker = new CoinPairTicker(logger);
            //ethTicker.Pair = pair;

            //ethTicker.PriceChange += ethMonitor.OnPriceChange;


            //ethTicker.UpdateTicker(3);

            //var repository = new DataRepository();

            Config.LoadConfiguration(logger);
            //var repo = new DataRepository();

            //var runner = new PatternRunner(logger,repo);

            //SqlConnection con = new SqlConnection(Config.SqlConnectionString);

            //con.Open();
            //foreach (var item in runner.PatternRepository)
            //{
            //    var p = new PatternView(item.Value);
            //    var commandString = string.Format(@"EXECUTE [dbo].[SavePatterns] '{0}', '{1}', '{2}', {3}, {4}", p.Name, p.Symbol, p.Interval, p.DefaultStopLossThreshold, p.DynamicStopLossThreshold);
            //    SqlCommand command = new SqlCommand(commandString, con);
            //    logger.Log(command.ToString());
            //    command.ExecuteNonQuery();
            //}
            //con.Close();

            //string[] exchanges = { "BNB", "CMC" };
            //var rabbitClient = new RabbitClient(logger, "TestQueue", exchanges, repository);
            //rabbitClient.Connect();

            //var pFactory = new PatternFactory(logger);
            //var data = new DataHandler(logger);

            //var coinData = data.LoadCoinDataFromCsv(@"C:\Users\gkaha\Dropbox\Crypto\POABTC_30m.csv");

            //Parser parser = new Parser(logger);
            //var klineList = parser.ParseKlinesFromCsvToList(coinData);

            //var pat = new SpringPattern(logger);
            //PatternEngine pEngine = new PatternEngine(logger);
            //pEngine.Patterns.Add(pat);


            //pEngine.CalculatePatternsFromDataFeed(klineList, pEngine.Patterns);

            //var p = new SpringPattern(logger, "ETHBTC", "15m");

            //decimal[] prices = new decimal[] { 5.4m, 5, 6.1m, 6.11m, 7, 6.998m, 6.9m, 6.5m, 6.55m, 6.6m, 6.7m, 6.9m, 6.997m, 6.99m, 7.1m, 7.5m, 7.3m, 7.2m, 7.1m, 7.2m, 7.5m, 6, 7 };
            //////decimal[] prices = new decimal[] { 5, 7, 9, 8, 10 };

            //foreach (var price in prices)
            //{
            //    p.CheckPattern(price, 1234);
            //}



            //BnbImporter
            //Config.SqlConnectionString = "Data Source=KAHANSKY;Initial Catalog=Crypto;User Id=CryptoAdmin;Password=CryptoAdmin";


            //MetaDataContainer.KlineQueue = new Queue<List<Kline>>();
            var rabbit = new RabbitHandler(logger, "BNB");
            var dbl = new DbHandler(logger);
            //MetaDataContainer.KlineQueue = new Queue<List<Kline>>();
            //MetaData meta = new MetaData();

            _dbHandler = dbl;
            //Config.LoadConfiguration(logger);

            MetaDataContainer.KlineQueue = new Queue<List<Kline>>();
            var bnb = new BnbCommunicator(logger, _dbHandler, rabbit);
            bnb.UpdateTickerPrices();
            bnb.SaveCandleStickData();
            Console.ReadKey();

        }

        private static void EthTicker_PriceChange(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
