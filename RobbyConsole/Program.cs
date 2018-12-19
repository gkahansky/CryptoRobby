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
//using CryptoRobert.RuleEngine;
using M3C.Finance.BinanceSdk;
using M3C.Finance.BinanceSdk.Methods;
using System.Data.SqlClient;
using CryptoRobert.Infra;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using RabbitMQ.Client;
using CryptoRobert.Trading;
using CryptoRobert.RuleEngine.Entities.Rules;
using CryptoRobert.RuleEngine.Interfaces;
using CryptoRobert.RuleEngine.BusinessLogic;

namespace RobbyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Prerequisites
            ILogger logger = new Logger("CryptoConsole");
            IRuleCalculator calculator = new RuleCalculator(logger);
            var priceRepo = new PriceRepository();
            #endregion

            var rule = new RulePriceTrend("BTCUSDT", "5m", 3, 1, "RulePriceTrend", calculator, priceRepo);
            var ruleType = rule.GetType().AssemblyQualifiedName;
            Console.WriteLine(ruleType);

            string p1 = "CryptoRobert.RuleEngine.Entities.Rules.";
            string p2 = ", RuleEngine, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null";
            string name = "RulePriceTrend";

            Type t = Type.GetType(p1 + name + p2);
            

            //("CryptoRobert.RuleEngine.Entities.Rules.RulePriceTrend");
            Console.WriteLine("RuleType is: " + t.Name);

            Console.ReadKey();
        }
    }
}
