using Crypto.Infra.Trading;
using CryptoRobert.Infra;
using CryptoRobert.Infra.Trading;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Infra.Patterns
{
    public abstract class Pattern : IPattern
    {
        JObject Settings { get; set; }
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public string Name { get; set; }
        public decimal HighPrice { get; set; }
        public decimal DefaultStopLoss { get; set; }
        public decimal DefaultStopLossThreshold { get; set; }
        public decimal DynamicStopLossThreshold { get; set; }
        public decimal DynamicStopLoss { get; set; }
        public enum PriceForCalc { AvgClose, Close, High, Low, Open, AvgOC, avgHL }
        public ITradeEngine Engine { get; set; }
        private ILogger logger;
        public  int Retention { get; set; }
        public decimal Threshold { get; set; }


        public Pattern(PatternConfig settings, ILogger _logger, string engineName="Generic")
        {
            logger = _logger;
            Symbol = settings.Symbol;
            Interval = settings.Interval;
            Name = settings.Name;
            Engine = new TradeEngine(logger,engineName);
        }

        public void UpdateSettings(PatternConfig config)
        {
            this.Name = config.Name;
            this.Interval = config.Interval;
            this.DefaultStopLossThreshold = config.DefaultStopLoss;
            this.DynamicStopLoss = config.DynamicStopLoss;
            this.Symbol = config.Symbol;
        }

        //public abstract bool CheckPattern(decimal avgPrice, long time);

        public abstract int CheckPattern(Kline kline);

        public abstract void SetHighPrice(decimal price);

        public abstract PriceForCalc DefinePriceForCalculation(IPattern p);

        public void ReportPatternStats()
        {
            if(Engine.Transactions.Count()==0)
            {
                Engine.MaxProfit = 0;
                Engine.MinProfit = 0;
            }
            string[] stats = 
                {
                Symbol,
                Name,
                Interval,
                Retention.ToString(),
                Threshold.ToString(),
                DefaultStopLossThreshold.ToString(),
                DynamicStopLossThreshold.ToString(),
                (Engine.TradeResults.Sum()).ToString(),
                Engine.TradeResults.Count().ToString(),
                Engine.MaxProfit.ToString(),
                Engine.MinProfit.ToString(),
                Engine.FirstTransactionTime.ToString(),
                Engine.LastTransactionTime.ToString(),
                };
            string msg = string.Join(",", stats);
            logger.Stats(msg);
        }
    }
}
