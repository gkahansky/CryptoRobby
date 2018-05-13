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
            Config.SqlConnectionString = "Data Source=KAHANSKY;Initial Catalog=Crypto;User Id=CryptoAdmin;Password=CryptoAdmin";
            var logger = new Logger("Robby");
            var dbl = new DbHandler(logger);
            //Config.LoadConfiguration(_logger);
            var bnb = new BnbCommunicator(logger, dbl);
            //var users = dbl.LoadUsers();
            var klines = bnb.GetCandleStickData("NANOBTC", "1h", 15);
            
            foreach(var k in klines)
            {
                logger.Log(String.Format(" OpenTime: {0}\n Open: {1}\n Close: {2}\n High: {3}\n Low: {4}\n CloseTime: {5}\n Interval: {6}",
                    k.OpenTime.ToString(), k.Open.ToString(), k.Close.ToString(), k.High.ToString(), k.Low.ToString(), k.CloseTime.ToString(), k.Interval.ToString()));
            }
            Console.ReadLine();
        }

    }
}
