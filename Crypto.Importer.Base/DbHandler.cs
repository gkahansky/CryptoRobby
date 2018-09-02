using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Data;
using Crypto.Infra.MarketData;

namespace CryptoRobert.Importer.Base
{
    public class DbHandler : IDbHandler
    {
        private readonly ILogger _logger;
        private MetaData metaData { get; set; }

        public DbHandler(ILogger logger)
        {
            _logger = logger;
            metaData = new MetaData();
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.AutoReset = true;
            timer.Enabled = true;

            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            while (MetaDataContainer.KlineQueue.Count > 0)
            {
                var list = MetaDataContainer.KlineQueue.Dequeue();
                //SaveKlines(list);
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

        public Dictionary<CoinInterval, List<TickGap>> FindMissingTicks(Dictionary<string, long> intervals)
        {
            try
            {
                var gaps = new Dictionary<CoinInterval, List<TickGap>>();
                var keys = FindUniqueKeysInKlinesTable();

                foreach (var key in keys)
                {
                    var gapList = FindGapsForCoinInterval(key, intervals);
                    if (gapList.Count() > 0)
                    {
                        _logger.Info(string.Format("Found {0} gaps for {1}-{2}", gapList.Count(), key.Symbol, key.Interval));
                        gaps.Add(key, gapList);
                    }
                        
                }

                return gaps;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to Retrieve missing ticks from database." + e);
                return null;
            }
        }

        private List<TickGap> FindGapsForCoinInterval(CoinInterval key, Dictionary<string, long> intervals)
        {
            var gapList = new List<TickGap>();
            var ticks = GetAllTicksForCoinInterval(key);
            long openTime = 0;
            long lastOpenTime = 0;
            var interval = intervals[key.Interval];
            foreach (var tick in ticks)
            {
                lastOpenTime = openTime;
                openTime = tick.OpenTime;
                if (openTime - lastOpenTime > interval && lastOpenTime > 0)
                {
                    var gap = new TickGap(lastOpenTime + interval, openTime - interval);
                    gapList.Add(gap);
                }
            }

            return gapList;
        }

        private List<Kline> GetAllTicksForCoinInterval(CoinInterval key)
        {
            using (var context = new InfraContext())
            {
                var klineList = context.Klines.Where(i => i.Symbol == key.Symbol && i.Interval == key.Interval).OrderBy(i => i.OpenTime).ToList();
                return klineList;
            }

        }

        private List<CoinInterval> FindUniqueKeysInKlinesTable()
        {
            var keyList = new List<CoinInterval>();
            using (var context = new InfraContext())
            {
                var data = context.Klines.Select(i => new { i.Symbol, i.Interval }).Distinct().ToList();
                foreach (var item in data)
                {
                    var coinKey = new CoinInterval(item.Symbol, item.Interval);
                    keyList.Add(coinKey);
                }
            }
            return keyList;
        }
        #endregion



    }
}
