using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;



namespace Crypto.Infra
{
    public class Parser
    {
        private readonly ILogger _logger;
        public Parser(ILogger logger)
        {
            _logger = logger;
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
                _logger.Log("Failed to Parse MarketData Statistics.\n" + e.ToString());
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
                _logger.Log("Failed to parse text to json.\n" + e.ToString());
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
                _logger.Log("Failed to Parse klines to Jsons.\n" + e.ToString());
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
                _logger.Log("Failed to Parse JObject to CandleSticks.\n" + e.ToString());
                return null;
            }
        }
    }


}
