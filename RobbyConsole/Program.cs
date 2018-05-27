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
            
            var last = DateTime.Now;
            Console.WriteLine(last);

            Thread.Sleep(3000);
            var diff = DateTime.Now - last;
            var span = new TimeSpan(0, 0, 3);
            var span2 = new TimeSpan(0, 0, 5);
            Console.WriteLine(diff);
            if (diff > span)
                Console.WriteLine("TRUE");
            if(diff<span2)
                Console.WriteLine("FALSE");



            //var symbol = "ETHBTC";
            //var interval = "1h";
            Config.SqlConnectionString = "Data Source=KAHANSKY;Initial Catalog=Crypto;User Id=CryptoAdmin;Password=CryptoAdmin";
            var logger = new Logger("Robby");

            MetaDataContainer.KlineQueue = new Queue<List<Kline>>();

            var dbl = new DbHandler(logger);

           MetaDataContainer.KlineQueue = new Queue<List<Kline>>();
            MetaData meta = new MetaData();


            Config.LoadConfiguration(logger);

            //CoinPair pair = new CoinPair();
            //pair.Id = 1;
            //pair.Symbol = "BNBBTC";
            //pair.Value = 123;
            //Config.PairsOfInterest.Add(pair);

            var bnb = new BnbCommunicator(logger, dbl);
            bnb.UpdateTickerPrices();
            bnb.SaveCandleStickData();
            Console.ReadKey();
            //var kline = new Kline();
            //kline.Symbol = "LRCBTC";
            //kline.Interval = "1w";
            //kline.OpenTime = 1514764800000;
            //kline.CloseTime = 1515369599999;
            //kline.Open = 0.00003280m;
            //kline.Close = 0.00007105m;
            //kline.High = 0.00008360m;
            //kline.Low = 0.00002890m;
            //kline.Volume = 6150.33545379m;

            //var klines = new List<Kline>();
            //klines.Add(kline);
            //dbl.SaveKlines(klines);

            ////var users = dbl.LoadUsers();
            ////var klines = bnb.GetCandleStickData(symbol, interval, 0, 1525726800000, 1526500800000);
            //logger.Log("Retrieving data for symbol: " + symbol + ", Interval: " + interval + 
            //    "\n OpenTime, Open, Close, High, Low, Volume, CloseTime");
            //foreach(var k in klines)
            //{
            //    logger.Log(String.Format("{0},{1},{2},{3},{4},{5},{6}",
            //          k.OpenTime.ToString()
            //        , k.Open.ToString()
            //        , k.Close.ToString()
            //        , k.High.ToString()
            //        , k.Low.ToString()
            //        , k.Volume.ToString()
            //        , k.CloseTime.ToString()));
            //}
            //Console.ReadLine();
        }

    }
}
