using CryptoRobert.Infra.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using CryptoRobert.Infra;
using System.IO;

namespace DataExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            var klines = new List<Kline>();
            Console.WriteLine("Choose File Name:");
            var fileName = Console.ReadLine();

            var path = @"C:\Crypto\BackTesting\" + fileName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            var writer = new Writer(path);
            var extractor = new Extractor();

            klines = extractor.Find("BTCUSDT", null, null,null);
            writer.Write("Symbol,Interval,OpenTime,CloseTime,Open,Close,High,Low,Volume");

            foreach (var k in klines)
            {
                string[] textArray = { k.Symbol, k.Interval, k.OpenTime.ToString(), k.CloseTime.ToString(), k.Open.ToString(), k.Close.ToString(), k.High.ToString(), k.Low.ToString(), k.Volume.ToString() };
                var text = string.Join(',', textArray);
                writer.Write(text);
            }

            Console.WriteLine("Done!");
        }
    }
}
