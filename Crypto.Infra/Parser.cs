using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace CryptoRobert.Infra
{
    public class Parser
    {
        private readonly ILogger _logger;
        public Parser(ILogger logger)
        {
            _logger = logger;
        }

        public static DateTime ConvertTimeMsToDateTime(long timeInMs)
        {
            var date = (new DateTime(1970, 1, 1)).AddMilliseconds(timeInMs);
            return date;
        }

        public static long ConvertTimeDateTimeToMs(DateTime date)
        {
            TimeSpan span = (date - new DateTime(1970, 1, 1));
            long ms = 0;
            var msString = span.TotalMilliseconds.ToString();
            if (msString.Contains("."))
            {
                var msClean = msString.Substring(0, msString.IndexOf('.'));
                ms = long.Parse(msClean);
            }
            else
                ms = long.Parse(msString);
            return ms;
        }


        public List<JObject> ParseCmcCoins(List<string> coinSetList)
        {
            var list = new List<JObject>();

            foreach (var coinSet in coinSetList)
            {
                var newString = coinSet.Substring(coinSet.IndexOf('{'), coinSet.LastIndexOf('}') - coinSet.IndexOf('{') + 1);
                var jsons = newString.Split('{');

                foreach (var json in jsons)
                {
                    if (json.Length > 2)
                    {
                        var newJson = "{" + json.Substring(0, json.LastIndexOf('}') + 1);
                        var newObj = JObject.Parse(newJson);
                        list.Add(newObj);
                    }
                }
            }
            return list;
        }

        public GlobalMarketData ParseCmcMarketData(string mdString)
        {
            try
            {
                var mdJson = JObject.Parse(mdString);
                var md = new GlobalMarketData
                {
                    Id = 1,
                    MarketDataUsd = Decimal.Parse(mdJson["total_market_cap_usd"].ToString()),
                    Volume24Hours = Decimal.Parse(mdJson["total_24h_volume_usd"].ToString()),
                    BitcoinDominancePct = Decimal.Parse(mdJson["bitcoin_percentage_of_market_cap"].ToString()),
                    ActiveCurrencies = int.Parse(mdJson["active_currencies"].ToString()),
                    ActiveAssets = int.Parse(mdJson["active_assets"].ToString()),
                    ActiveMarkets = int.Parse(mdJson["active_markets"].ToString()),
                    //LastUpdate = DateTime.Parse(mdJson["last_updated"].ToString())
                };
                return md;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to Parse MarketData Statistics.\n" + e.ToString());
                return null;
            }
        }

        public JObject ParseTextToJson(string configString)
        {
            try
            {
                var json = JObject.Parse(configString);
                return json;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to parse text to json.\n" + e.ToString());
                return null;
            }
        }

        public List<Kline> ParseKlinesFromCsvToList(List<string> list, string symbol = null, string interval = null)
        {
            try
            {
                var klineList = new List<Kline>();
                foreach (var line in list)
                {
                    if (!line.StartsWith("Symbol"))
                    {
                        var split = line.Split(',');
                        var kline = new Kline();
                        if ((string.IsNullOrEmpty(symbol) || symbol == split[0].ToString()) && (string.IsNullOrEmpty(interval) || interval == split[1]))
                        {
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
                        //Symbol,Interval,OpenTime,CloseTime,Open,Close,High,Low,Volume
                    }
                }

                return klineList;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to parse klines from text to kline list.\n" + e.ToString());
                return null;
            }
        }

        public List<CoinPair> ParseBnbTickers(string tickers)
        {
            try
            {
                var tickerList = ConvertBnbResponseToArray(tickers);
                var pairList = new List<CoinPair>();

                foreach (var ticker in tickerList)
                {
                    var json = JObject.Parse(ticker);
                    var pair = new CoinPair();

                    pair.Symbol = json["symbol"].ToString();
                    pair.Value = decimal.Parse(json["price"].ToString());
                    pair.LastUpdate = new Dictionary<string, long>();
                    pairList.Add(pair);
                }

                _logger.Info(String.Format("{0} pairs received from Binance", pairList.Count));

                return pairList;
            }
            catch (Exception e)
            {
                _logger.Error("Parser failed to parse coin pairs from text to CoinPair list.\n" + e.ToString());
                return null;
            }
        }

        public List<Kline> ConvertKlineStringToList(string response, string interval, string symbol)
        {
            List<string[]> klineList = new List<string[]>();
            string[] klineArray = response.Substring(1, response.Length - 1).Split('[');
            foreach (var kline in klineArray)
            {
                if (kline.Length > 2)
                {
                    klineList.Add(kline.Substring(0, kline.Length - 2).Split(','));
                }

            }

            var jsons = ConvertKlineArraystoJobject(klineList, symbol, interval);

            var list = ConvertJsonsToKlines(jsons, symbol, interval);
            return list;
        }

        private List<JObject> ConvertKlineArraystoJobject(List<string[]> stringArrays, string symbol, string interval)
        {
            try
            {
                var jsons = new List<JObject>();
                foreach (var s in stringArrays)
                {
                    var kline = new JObject();
                    kline.Add("Symbol", symbol);
                    kline.Add("Interval", interval);
                    kline.Add("OpenTime", JToken.Parse(s[0]));
                    kline.Add("Open", JToken.Parse(s[1]));
                    kline.Add("High", JToken.Parse(s[2]));
                    kline.Add("Low", JToken.Parse(s[3]));
                    kline.Add("Close", JToken.Parse(s[4]));
                    kline.Add("Volume", JToken.Parse(s[5]));
                    kline.Add("CloseTime", JToken.Parse(s[6]));
                    jsons.Add(kline);
                }

                return jsons;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to Parse klines to Jsons.\n" + e.ToString());
                return null;
            }
        }

        private List<Kline> ConvertJsonsToKlines(List<JObject> jsons, string symbol, string interval)
        {
            try
            {
                var klines = new List<Kline>();
                //var klineInterval = EnumCollection.ConvertIntervalStringToEnum(interval);

                foreach (var j in jsons)
                {
                    var kline = new Kline();
                    kline.Symbol = symbol;
                    kline.Interval = interval;
                    kline.OpenTime = long.Parse(j["OpenTime"].ToString());
                    kline.Open = decimal.Parse(j["Open"].ToString());
                    kline.High = decimal.Parse(j["High"].ToString());
                    kline.Low = decimal.Parse(j["Low"].ToString());
                    kline.Close = decimal.Parse(j["Close"].ToString());
                    kline.Volume = decimal.Parse(j["Volume"].ToString());
                    kline.CloseTime = long.Parse(j["CloseTime"].ToString());

                    klines.Add(kline);
                }
                return klines;


            }
            catch (Exception e)
            {
                _logger.Error("Failed to Parse JObject to CandleSticks.\n" + e.ToString());
                return null;
            }
        }

        private List<string> ConvertBnbResponseToArray(string response)
        {
            var end = response.Length - 1;
            var resClean = response.Substring(1, end);
            var jsonstrings = resClean.Replace('}', '^');
            var splitJsons = jsonstrings.Split('^');
            var pairList = new List<string>();
            foreach (var pair in splitJsons)
            {
                string pairNew;
                if (pair.StartsWith(",{"))
                {
                    if (!pair.EndsWith("}"))
                        pairNew = pair.Substring(1) + "}";
                    else
                        pairNew = pair.Substring(1);

                    pairList.Add(pairNew);
                }
            }
            return pairList;
        }
    }

}
