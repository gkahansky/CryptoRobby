using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Data;
using System.Timers;
using CryptoRobert.Infra.Rabbit;

namespace CryptoRobert.DBLoader
{

    public class DbHandler : IDbHandler
    {
        private readonly ILogger _logger;
        public DataRepository KlineRepository;
        private List<Kline> klineList { get; set; }
        private bool timeToSave { get; set; }
        //private InfraContext context;

        public DbHandler(ILogger logger, DataRepository repository)
        {
            _logger = logger;
            klineList = new List<Kline>();
            KlineRepository = repository;
            //context = new InfraContext();
            //Timer timer = new Timer(1000);
            //timer.AutoReset = true;
            //timer.Enabled = true;
            
            //timer.Elapsed += Timer_Elapsed;
        }

        //private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    if (klineList.Count() > 0)
        //    {
        //        _logger.Info(string.Format("Saving {0} klines", klineList.Count()));
        //        SaveKlines(klineList);
        //    }
        //}


        public void OnKlineReceived(object source, EventArgs e)
        {
            var kline = KlineRepository.Klines.Peek();
            SaveKline(kline);
            KlineRepository.Klines.Dequeue();
            _logger.Debug("Kline Dequeued");
            //var kline = (KlineRepository.Klines.Peek());
            //SaveKline(kline);
            //_logger.Debug(string.Format("Kline Received: {0}, {1}, {2}, {3}", kline.Symbol, kline.Interval, kline.OpenTime, kline.Close));
            //KlineRepository.Klines.Dequeue();
        }

        private void SaveKline(Kline kline)
        {
            _logger.Debug("kline received for " + kline.Symbol + "-" + kline.Interval);
            using (var context = new InfraContext())
            {
                try
                {
                    context.Klines.Add(kline);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    _logger.Error("Failed to save kline.\n" + e);
                }
            }
        }


        #region CMC Methods
        public void SaveCoin(string symbol, string name)
        {
            try
            {
                var coin = new Coin(symbol, name);
                using (var context = new InfraContext())
                {
                    var coins = from c in context.Coins
                                where c.Symbol == coin.Symbol
                                select c;

                    if (coins.Count() == 0)
                    {
                        context.Coins.Add(coin);
                        context.SaveChanges();
                        _logger.Info(String.Format("New Coin Saved: Id={0}, Symbol = {1}, Name={2}", coin.Id, coin.Symbol, coin.Name));
                    }
                }

            }
            catch (Exception e)
            {
                _logger.Error("Failed to save coin to database.\n" + e.ToString());
            }
        }

        public void SaveMarketData(GlobalMarketData newMd)
        {
            using (var context = new InfraContext())
            {
                context.Set<GlobalMarketData>().Update(newMd);
                context.SaveChanges();
                //Logger.Log(String.Format("New Coin Saved: Id={0}, Symbol = {1}, Name={2}", coin.Id, coin.Symbol, coin.Name));
            }
        }
        #endregion

        #region Binance Methods
        public void SaveCoinPairs(List<CoinPair> tickers)
        {
            int numOfRows = 0;
            using (var context = new InfraContext())
            {
                foreach (var ticker in tickers)
                {
                    numOfRows += context.Database.ExecuteSqlCommand
                        ("UpdateCoinPairs @Symbol={0}, @Value={1}",
                        ticker.Symbol, ticker.Value);
                }
            }
            _logger.Info(String.Format("{0} Coin Pairs updated in database", numOfRows * -1));
        }

        public void SaveKlines(List<Kline> klines)
        {
            
            var numOfKlines = klines.Count();
            _logger.Info(string.Format("{0} updates received, saving data to database",numOfKlines));
            //if (numOfKlines > 0)
            //{
                try
                {
                    using (var context = new InfraContext())
                    {
                        context.Klines.AddRange(klines);
                        //klines.RemoveRange(0, numOfKlines);
                        context.SaveChanges();
                        context.Klines.RemoveRange(klines);
                }
                _logger.Info(String.Format("{0} klines updated in database for {1} {2}", klines.Count(), klines[0].Symbol, klines[0].Interval));
                }
                catch (Exception e)
                {
                    _logger.Error(String.Format("Failed to save klines to database. Symbol: {0}, Interval: {1}.\n{3}", klines[0].Symbol, klines[0].Interval, e.ToString()));
                }
                
            //}

        }
        
        #endregion

        #region Generic Methods

        public long FindKlineLastUpdate(string symbol, string interval)
        {
            try
            {
                long lastupdate = 0;
                using (var context = new InfraContext())
                {
                    try
                    {
                        lastupdate = context.Klines.Where(a => a.Symbol == symbol && a.Interval == interval).Max(a => a.CloseTime);
                    }
                    catch (Exception)
                    {
                        lastupdate = 0;
                    }


                    //var last = context.Klines.FromSql("GetLastUpdateBySymbolAndInterval @Symbol = {0}, @Interval = {1}", symbol, interval);
                    return lastupdate;
                }

            }
            catch (Exception e)
            {
                _logger.Error("Failed to load last update date for kline.\n" + e.ToString());
                return -1;
            }
        }

        public Dictionary<string, Coin> LoadCoins()
        {
            var coins = new Dictionary<string, Coin>();
            using (var context = new InfraContext())
            {
                var coinList = from c in context.Coins
                               select c;
                foreach (var coin in coinList)
                {
                    coins.Add(coin.Name, coin);
                }
            }
            return coins;

        }

        #endregion



    }
}
