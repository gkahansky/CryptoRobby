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
