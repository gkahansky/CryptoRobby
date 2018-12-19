using CryptoRobert.Infra;
using CryptoRobert.RuleEngine.BusinessLogic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleTester
{
    public class Executor
    {
        ILogger logger;
        Parser parser;
        RuleValidator runner;

        public Executor(ILogger _logger, RuleValidator validator)
        {
            logger = _logger;
            parser = new Parser(logger);
            runner = validator;
        }

        public void Execute(int mode, string path = null)
        {
            var klines = new List<Kline>();
            if (mode == 0 && !string.IsNullOrEmpty(path))
                klines = GetKlinesFromFile(path);
            else
                klines = GetKlinesFromDb();

            RunTest(klines);
        }

        private void RunTest(List<Kline> klines)
        {
            foreach(var kline in klines)
            {
                runner.ProcessKline(kline);
            }
        }

        private List<Kline> GetKlinesFromFile(string path)
        {
            var klinesText = File.ReadAllLines(path);
            var klines = ConvertKlineTextToKlineList(klinesText);
            
            return klines;
        }

        private List<Kline> ConvertKlineTextToKlineList(string[] klinesText)
        {
            var klineList = new List<Kline>();
            foreach (var line in klinesText)
            {
                if (!line.StartsWith("Symbol"))
                {
                    var split = line.Split(',');
                    var kline = new Kline();

                    kline.Symbol = split[0];
                    kline.Interval = split[1];
                    kline.OpenTime = long.Parse(split[2]);
                    kline.CloseTime = long.Parse(split[3]);
                    kline.Open = decimal.Parse(split[4]);
                    kline.Close = decimal.Parse(split[5]);
                    kline.High = decimal.Parse(split[6]);
                    kline.Low = decimal.Parse(split[7]);
                    kline.Volume = decimal.Parse(split[8]);
                    klineList.Add(kline);
                }
            }
            return klineList;
        }

        private List<Kline> GetKlinesFromDb()
        {
            throw new NotImplementedException();
        }
    }
}
