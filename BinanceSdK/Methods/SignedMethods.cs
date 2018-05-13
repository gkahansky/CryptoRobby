using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Crypto.Infra;
using M3C.Finance.BinanceSdk.ResponseObjects;
using M3C.Finance.BinanceSdk.Enumerations;

namespace M3C.Finance.BinanceSdk.Methods
{
    public static class SignedMethods
    {

        public static BinanceClient InitRestClient(string apiKey, string secretKey)
        {
            //You need to init client with Api Keys for Signed Endpoint requests
            var restClient = new BinanceClient(apiKey, secretKey);
            //Logger.Log("Binance Client initiated succesfully");
            return restClient;
        }

        public static AccountResponse GetAccountInfo(BinanceClient restClient)
        {
            //Get Account Info and Current Balances
            var accountInfo = restClient.GetAccountInfo(true).Result;
            //Logger.Log($"Maker/Taker Commission: {accountInfo.MakerCommission}/{accountInfo.TakerCommission}");
            //foreach (var balanceItem in accountInfo.Balances)
            //{
            //    Logger.Log($"{balanceItem.Asset} Total Amount: {balanceItem.Free + balanceItem.Locked}");
            //}
            return accountInfo;
        }

        public static void BuyMarketPrice(BinanceClient restClient, string pair, int buySell, decimal quantity, decimal price)
        {

            //{ "symbol", symbol}, //pair
            //    { "side", side}, //buy
            //    { "type", orderType}, //market
            //    { "timeInForce", timeInForce}, //default
            //    { "quantity", quantity.ToString(CultureInfo.InvariantCulture)}, //from wallet
                

            //string orderId = pair + DateTime.Now.ToString("yyyyMMdd_hhmmss");
            //Initiates an order. 
            //Ensure that isTestOrder parameter is set to false for real order requests (By default it is false).
            //ClientOrderId is optional. It is useful if you might need follow up requests like cancel etc. If you dont set BinanceApi assigns one for the order.
            //var orderResult = restClient.NewMarketOrder(pair, OrderSide.Buy, OrderType.Limit, TimeInForce.GoodUntilCanceled, quantity, price, false, orderId).Result;
            //Console.WriteLine($"Accepted with Id: {orderResult.OrderId} @ Time: {orderResult.TransactionTime}");

        }

    }
}
