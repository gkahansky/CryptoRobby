//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CryptoRobert.Infra.Trading
//{
//    public class Transaction
//    {
//        public string Symbol { get; set; }
//        public decimal BuyPrice { get; set; }
//        public decimal HighPrice { get; set; }
//        public StopLossDefinition StopLossConfig { get; set; }

//        public Transaction(string symbol, decimal price)
//        {
//            BuyPrice = price;
//            HighPrice = price;
//            Symbol = symbol;
//        }



//        private void CalculateDefaultStopLoss(decimal price)
//        {
//            if (StopLossConfig.DefaultStopLossThreshold > 0)
//                StopLossConfig.DefaultStopLoss = BuyPrice - (BuyPrice * StopLossConfig.DefaultStopLossThreshold);
//            else
//                StopLossConfig.DefaultStopLoss = BuyPrice - (BuyPrice * 0.05m);
//        }

//        private void CalculateDynamicStopLoss(decimal price)
//        {
//            if (StopLossConfig.DynamicSLThreshold > 0)
//            {
//                if (HighPrice > (BuyPrice + BuyPrice * StopLossConfig.DynamicSLThreshold))
//                    StopLossConfig.DynamicStopLoss = HighPrice - (HighPrice * StopLossConfig.DynamicSLThreshold);
//                else
//                    StopLossConfig.DynamicStopLoss = StopLossConfig.DefaultStopLoss;
//            }
                
//            else
//                StopLossConfig.DynamicStopLoss = HighPrice - (HighPrice * 0.05m);
//        }

//        internal bool CheckStopLoss(Transaction t, decimal price)
//        {
//            CalculateStopLoss(price);

//            if (price < StopLossConfig.DefaultStopLoss) 
//                return true;

//            var profit = (t.HighPrice / t.BuyPrice) - 1;
//            if (price < StopLossConfig.DynamicStopLoss && profit > StopLossConfig.DynamicSLThreshold)
//                return true;

//            return false;
//        }

//        internal void CalculateStopLoss(decimal price)
//        {
//            CalculateDefaultStopLoss(price);
//            CalculateDynamicStopLoss(price);
//        }
//    }
//}
