using CryptoRobert.Infra;
using CryptoRobert.RuleEngine.Transactions;
using CryptoRobert.Infra.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine
{
    public class TradeEngine
    {
        public Dictionary<string, StopLossDefinition> StopLossCollection;
        public Dictionary<string, Transaction> Transactions;
        private ILogger _logger;

        public TradeEngine(ILogger logger)
        {
            StopLossCollection = new Dictionary<string, StopLossDefinition>();
            Transactions = new Dictionary<string, Transaction>();
            _logger = logger;
        }

        public void BuyPair(Kline kline, IPattern p, string name)/*Dictionary<string, PatternConfig> patternsConfig*/
        {
            if (Transactions.ContainsKey(kline.Symbol))
            {
                _logger.Log("Not Buying since we already baught it");
            }
            else
            {
                var t = new Transaction(kline.Symbol, kline.Close);
                t.StopLossConfig = GenerateStopLossObject(p);
                t.CalculateStopLoss(kline.Close);
                Transactions.Add(t.Symbol, t);
                var msg = String.Format("Trade: Buying {0} at {1}. Pattern: {2} Interval - {3}", t.Symbol, t.BuyPrice, p.Name, p.Interval);
                _logger.Email(string.Format("{0} Detected! Buy {1}", p.Name, t.Symbol), msg);
            }
        }

        public void Sell(string symbol, decimal price)
        {
            var t = Transactions[symbol];
            var profit = ((price / t.BuyPrice) - 1) * 100;
            var profitText = profit.ToString();
            if (profitText.Length > 5)
                profitText = profit.ToString().Substring(0, 5);
            var msg = string.Format("Trade: SELLING {0}!!! Buy Price: {1}, Sell Price: {2}, Profit: {3}%", t.Symbol, t.BuyPrice, price.ToString(), profitText);
            _logger.Email(string.Format("SELL notice! selling {0} at {1}% from buy price", t.Symbol, profitText), msg);
            Transactions.Remove(symbol);
        }

        public StopLossDefinition GenerateStopLossObject(IPattern p)//PatternConfig settings)
        {
            var sl = new StopLossDefinition();
            sl.DefaultStopLossThreshold = p.DefaultStopLossThreshold;
            sl.DynamicSLThreshold = p.DynamicStopLossThreshold;
            return sl;
        }

        public bool CheckStopLoss(IPattern p, Kline kline)
        {

            if (!Transactions.ContainsKey(kline.Symbol))
                return false;

            var t = Transactions[kline.Symbol];
            t.HighPrice = p.HighPrice;

            var sell = t.CheckStopLoss(t, kline.Close);

            return sell;
        }
    }
}
