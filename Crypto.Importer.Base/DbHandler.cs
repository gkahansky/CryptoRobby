using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Crypto.Infra;
using Crypto.Infra.Data;

namespace Crypto.Importer.Base
{
    public class DbHandler : IDbHandler
    {
        private readonly ILogger _logger;

        public DbHandler(ILogger logger)
        {
            _logger = logger;
         
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
                        _logger.Log(String.Format("New Coin Saved: Id={0}, Symbol = {1}, Name={2}", coin.Id, coin.Symbol, coin.Name));
                    }
                }

            }
            catch (Exception e)
            {
                _logger.Log("Failed to save coin to database.\n" + e.ToString());
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
            _logger.Log(String.Format("{0} Coin Pairs updated in database", numOfRows * -1));
        }

        public void SaveKlines(List<Kline> klines)
        {
            if (klines.Count > 0)
            {
                int numOfRows = 0;
                int state = 0;
                try
                {
                    
                    using (var context = new InfraContext())
                    {
                        foreach (var kline in klines)
                        {
                            context.Klines.Add(kline);
                            numOfRows += 1;
                        }
                        state = context.SaveChanges();
                    }
                    _logger.Log(String.Format("{0} klines updated in database", numOfRows));
                }
                catch (Exception e)
                {
                    _logger.Log(String.Format("Failed to save klines to database.\nSymbol: {0}, Interval: {1},\nOpen/Close Time: {2}/{3}\nOpen/Close: {4}/{5},\nHigh/Low: {6}/{7}\nVolume: {8}\n{9}",
                        klines[numOfRows].Symbol, klines[numOfRows].Interval
                        , klines[numOfRows].OpenTime, klines[numOfRows].CloseTime
                        , klines[numOfRows].Open, klines[numOfRows].Close
                        , klines[numOfRows].High, klines[numOfRows].Low
                        , klines[numOfRows].Volume
                        , e.ToString()));
                }
            }
            else
                _logger.Log("No new data received for current interval");
        }
        #endregion

        #region Generic Methods

        public IQueryable<User> LoadUsers()
        {
            using (var context = new InfraContext())
            {
                var users = from u in context.Users
                            select u;

                _logger.Log(String.Format("Found {0} Users in database", users.Count()));
                foreach (var user in users)
                {
                    _logger.Log(String.Format("UserName: {0}, UserId: {1}, API: {2}, Secret: {3}",
                        user.UserName, user.Id, user.BinanceAPI, user.BinanceSecret));
                }
                return users;
                //Logger.Log(String.Format("New Coin Saved: Id={0}, Symbol = {1}, Name={2}", coin.Id, coin.Symbol, coin.Name));
            }

        }

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
                _logger.Log("Failed to load last update date for kline.\n" + e.ToString());
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

        //public void SaveWalletsToDatabase(List<Wallet> wallets)
        //{
        //    if (wallets.Count() > 0)
        //    {

        //        using (var context = new InfraContext())
        //        {
        //            foreach (var w in wallets)
        //            {
        //                context.Database.ExecuteSqlCommand
        //                ("SaveWallet @Symbol={0}, @UserId={1}, @ExchangeId={2], @Free={3}, @Locked={4}",
        //                w.Symbol, w.UserId, w.Exchange, w.QuantityFree, w.QuantityLocked);
        //            }
        //        }
        //    }
        //}
        #endregion



    }
}
