using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M3C.Finance.BinanceSdk.Enumerations;
using M3C.Finance.BinanceSdk.ResponseObjects;
using Crypto.Infra;


namespace M3C.Finance.BinanceSdk.Methods
{
    public static class PublicMethods
    {
        private static readonly ILogger _logger;
        
        public static DepthResponse GetPairPrice(BinanceClient publicRestClient, string pair)
        {
            var depthResult = publicRestClient.Depth(pair).Result;
            var limitedDepthResult = publicRestClient.Depth(pair, 5).Result;
            //Logger.Log($"First Ask Price: {depthResult.Asks[0].Price}, First Ask Quantity: {depthResult.Asks[0].Quantity}");
            //Logger.Log($"NoLimit Record Count: {depthResult.Asks.Count}, Limited Record Count: {limitedDepthResult.Asks.Count}");

            return depthResult;
        }

        public static List<ResponseObjects.AggregateTradeResponseItem> GetPairInfo(BinanceClient publicRestClient, string pair)
        {
            //For gettings Compressed/Aggregate trades list
            var aggregateTrades = publicRestClient.AggregateTrades(pair).Result;
            //Logger.Log($"AggTradeId: {aggregateTrades[0].AggregateTradeId} Price: {aggregateTrades[0].Price}  Quantity: {aggregateTrades[0].Quantity}  WasMaker: {aggregateTrades[0].WasBuyerTheMaker}");

            return aggregateTrades;
        }

        public static void GetCandlestickData(BinanceClient publicRestClient, string pair, KlineInterval klineInterval)
        {
            //Get KLines/Candlestick data for given Trade Pair
            var kLines = publicRestClient.KLines(pair, klineInterval).Result;
            foreach (var item in kLines)
            {
                _logger.Log($"# of Trades: {item.NumberOfTrades} Close: {item.Close} Volume: {item.Volume}");
            }
        }

        public static void GetDailyTicker(BinanceClient publicRestClient, string pair)
        {
            //Get Daily Ticker for given Trade Pair
            var dailyTicker = publicRestClient.TickerDaily(pair).Result;
            _logger.Log($"Ask: {dailyTicker.AskPrice} Bid: {dailyTicker.BidPrice} Change: {dailyTicker.PriceChange}");

        }

        public static IEnumerable<TickerSummary> GetAllTickers(BinanceClient publicRestClient)
        {
            //Get All Ticker Information
            var allPricesTicker = publicRestClient.TickerAllPrices().Result;
            foreach (var item in allPricesTicker)
            {
                _logger.Log($"{item.Symbol} : {item.Price}");
            }

            return allPricesTicker;
        }

        public static void GettAllBookTickers(BinanceClient publicRestClient)
        {
            //Get AllBookTicker information with price and quantity data
            var allBookTickers = publicRestClient.AllBookTickers().Result;
            foreach (var item in allBookTickers)
            {
                _logger.Log($"{item.Symbol} : AskPrice/Quantity {item.AskPrice} / {item.AskQuantity} BidPrice/Quantity {item.BidPrice} / {item.BidQuantity}");
            }
        }
        


}
}
