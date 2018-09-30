using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Trading
{
    public class TradeEngine : ITradeEngine
    {
        public Dictionary<string, StopLossDefinition> StopLossCollection { get; }
        public  Dictionary<string, Transaction> Transactions { get; }
        public PriceRepository Prices { get; set; }
        private ILogger _logger;
        public string Name { get; set; }
        public List<decimal> TradeResults { get; set; }
        public decimal MaxProfit { get; set; }
        public decimal MinProfit { get; set; }
        public DateTime FirstTransactionTime { get; set; }
        public DateTime LastTransactionTime { get; set; }

        public TradeEngine(ILogger logger, PriceRepository prices, string name = "Generic")
        {
            StopLossCollection = new Dictionary<string, StopLossDefinition>();
            Transactions = new Dictionary<string, Transaction>();
            TradeResults = new List<decimal>();
            Prices = prices;
            _logger = logger;
            Name = name;
            MaxProfit = decimal.MinValue;
            MinProfit = decimal.MaxValue;
            FirstTransactionTime = DateTime.MinValue;
        }

        public void BuyPair(Kline kline, StopLossDefinition stopLoss)/*Dictionary<string, PatternConfig> patternsConfig*/
        {
            if (Transactions.ContainsKey(kline.Symbol))
            {
                _logger.Info("Not Buying since we already baught it");
            }
            else
            {
                var time = Parser.ConvertTimeMsToDateTime(kline.CloseTime);
                var t = new Transaction(kline.Symbol, kline.Close);
                t.StopLossConfig = stopLoss;
                t.CalculateStopLoss(kline.Close);
                Transactions.Add(t.Symbol, t);
                //var msg = String.Format("Trade: Buying {0} at {1}. Pattern: {2} Interval - {3}", t.Symbol, t.BuyPrice, p.Name, p.Interval);
                //_logger.Email(string.Format("{0} Detected! Buy {1}", p.Name, t.Symbol), msg);
                if (FirstTransactionTime == DateTime.MinValue)
                    FirstTransactionTime = time;
            }
        }

        public void Sell(Kline kline, out decimal profit)
        {
            profit = decimal.MinValue;
            var symbol = kline.Symbol;
            var price = kline.Close;
            var time = Parser.ConvertTimeMsToDateTime(kline.CloseTime);

            if (Transactions.Count > 0 && Transactions.ContainsKey(symbol))
            {
                var t = Transactions[symbol];
                profit = ((price / t.BuyPrice) - 1);
                var profitText = (profit*100).ToString();
                if (profitText.Length > 6)
                    profitText = profit.ToString().Substring(0, 5);
                var msg = string.Format("Trade: SELLING {0}!!! Buy Price: {1}, Sell Price: {2}, Profit: {3}%", t.Symbol, t.BuyPrice, price.ToString(), profitText);
                _logger.Email(string.Format("SELL notice! selling {0} at {1}% from buy price", t.Symbol, profitText), msg);
                Transactions.Remove(symbol);
                UpdateStats(profit, time);
            }
        }

        private void UpdateStats(decimal profit, DateTime time)
        {
            if (profit > MaxProfit)
                MaxProfit = profit;
            if (profit < MinProfit)
                MinProfit = profit;
            LastTransactionTime = time;
        }

        public bool CheckStopLoss(Kline kline)
        {

            if (!Transactions.ContainsKey(kline.Symbol))
                return false;

            var t = Transactions[kline.Symbol];

            var sell = t.CheckStopLoss(t, kline.Close);

            return sell;
        }

    }
}
