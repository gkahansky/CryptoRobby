using CryptoRobert.Infra;
using CryptoRobert.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataExtractor
{
    public class Extractor
    {
        public List<Kline> Find(string symbol = null, string interval = null, string from = null, string to = null)
        {
            var klines = new List<Kline>();

            if (symbol == null && interval == null && from == null && to == null)
                klines = FindAll();
            else if (symbol != null && interval == null && from == null && to == null)
                klines = FindBySymbol(symbol);
            else if (symbol != null && interval != null && from == null && to == null)
                klines = FindBySymbolInterval(symbol, interval);
            else if (symbol != null && interval != null && from != null && to != null)
                klines = FindBySymbolIntervalTime(symbol, interval, from, to);
            else if (symbol == null && interval == null && from != null && to != null)
                klines = FindByTime(from, to);
            else if (symbol != null && interval == null && from != null && to != null)
                klines = FindByIntervalTime(interval, from, to);
            else if (symbol == null && interval != null && from == null && to == null)
                klines = FindByInterval(interval);
            else
                return null;

            return klines;
        }

        private List<Kline> FindByInterval(string interval)
        {
            List<Kline> klines = new List<Kline>();
            Console.WriteLine(string.Format("Retreiving ticks from database for {0}", interval));
            using (var context = new InfraContext())
            {
                klines = context.Klines.Where(a => a.Interval == interval).OrderBy(a => a.OpenTime).ToList();
            }
            return klines;
        }

        public List<Kline> FindAll()
        {
            List<Kline> klines = new List<Kline>();
            Console.WriteLine("Retreiving ALL ticks from database");
            using (var context = new InfraContext())
            {
                klines = context.Klines.OrderBy(a => a.OpenTime).ToList();
            }
            return klines;
        }

        public List<Kline> FindBySymbol(string symbol)
        {
            List<Kline> klines = new List<Kline>();
            Console.WriteLine(string.Format("Retreiving ticks from database for {0}", symbol));
            using (var context = new InfraContext())
            {
                klines = context.Klines.Where(a => a.Symbol == symbol).OrderBy(a => a.OpenTime).ToList();
            }
            return klines;
        }

        public List<Kline> FindBySymbolInterval(string symbol, string interval)
        {
            List<Kline> klines = new List<Kline>();
            Console.WriteLine(string.Format("Retreiving ticks from database for {0} {1}", symbol, interval));
            using (var context = new InfraContext())
            {
                klines = context.Klines.Where(a => a.Symbol == symbol && a.Interval == interval).OrderBy(a => a.OpenTime).ToList();
            }
            return klines;
        }

        public List<Kline> FindByTime(string from, string to)
        {
            var fromTime = Parser.ConvertTimeMsToDateTime(long.Parse(from));
            var toTime = Parser.ConvertTimeMsToDateTime(long.Parse(to));
            var fromDecimal = decimal.Parse(from);
            var toDecimal = decimal.Parse(to);

            List<Kline> klines = new List<Kline>();
            Console.WriteLine(string.Format("Retreiving ticks from database from {0} to {1}", fromTime.ToString("yyyy-mm-dd hh:mm:ss"), toTime.ToString("yyyy-mm-dd hh:mm:ss")));
            using (var context = new InfraContext())
            {
                klines = context.Klines.Where(a => a.OpenTime >= fromDecimal && a.OpenTime <= toDecimal).OrderBy(a => a.OpenTime).ToList();
            }
            return klines;
        }

        public List<Kline> FindByIntervalTime(string interval, string from, string to)
        {
            var fromTime = Parser.ConvertTimeMsToDateTime(long.Parse(from));
            var toTime = Parser.ConvertTimeMsToDateTime(long.Parse(to));
            var fromDecimal = decimal.Parse(from);
            var toDecimal = decimal.Parse(to);

            List<Kline> klines = new List<Kline>();
            Console.WriteLine(string.Format("Retreiving ticks from database for {0} between {1}-{2}", interval, fromTime.ToString("yyyy-mm-dd hh:mm:ss"), toTime.ToString("yyyy-mm-dd hh:mm:ss")));
            using (var context = new InfraContext())
            {
                klines = context.Klines.Where(a => a.Interval == interval && a.OpenTime >= fromDecimal && a.OpenTime <= toDecimal).OrderBy(a => a.OpenTime).ToList();
            }
            return klines;
        }

        public List<Kline> FindBySymbolIntervalTime(string symbol, string interval, string from, string to)
        {
            var fromTime = Parser.ConvertTimeMsToDateTime(long.Parse(from));
            var toTime = Parser.ConvertTimeMsToDateTime(long.Parse(to));
            var fromDecimal = decimal.Parse(from);
            var toDecimal = decimal.Parse(to);

            List<Kline> klines = new List<Kline>();
            Console.WriteLine(string.Format("Retreiving ticks from database for {0} {1} between {2}-{3}", symbol, interval, fromTime.ToString("yyyy-mm-dd hh:mm:ss"), toTime.ToString("yyyy-mm-dd hh:mm:ss")));
            using (var context = new InfraContext())
            {
                klines = context.Klines.Where(a => a.Symbol == symbol && a.Interval == interval && a.OpenTime >= fromDecimal && a.OpenTime <= toDecimal).OrderBy(a => a.OpenTime).ToList();
            }
            return klines;
        }

    }
}
