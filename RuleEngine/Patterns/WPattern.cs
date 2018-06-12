//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Crypto.Infra;
//using Newtonsoft.Json.Linq;

//namespace Crypto.RuleEngine.Patterns
//{
//    public class WPattern : IPattern
//    {
//        private ILogger logger;
//        public bool Trend { get; set; }
//        public bool TrendShift { get; set; }
//        public decimal Low1 { get; set; }
//        public decimal Low2 { get; set; }
//        public decimal Peak { get; set; }
//        public decimal Pivot { get; set; }
//        public decimal Start { get; set; }
//        public decimal LastPrice { get; set; }
//        public decimal ThresholdLeg { get; set; } //what is the minimum range for a W legs
//        public decimal ThresholdTip { get; set; } //what is the minimum range for a W middle tip
//        public decimal ThresholdBase { get; set; } //what is the maximum range for a W floor - e.g. the diff between one floor and the other.


//        public WPattern(ILogger _logger, JObject config)
//        {
//            logger = _logger;
//            Trend = true;
//            Start = 0;
//            LastPrice = 0;
//            ThresholdLeg = decimal.Parse(config["ThresholdLeg"].ToString());
//            ThresholdTip = decimal.Parse(config["ThresholdTip"].ToString());
//            ThresholdBase = decimal.Parse(config["ThresholdBase"].ToString());
//        }

//        public bool CheckPattern(decimal price, long time)
//        {
//            if (Start == 0)
//                Start = SetStartPoint(price);

//            Trend = CheckTrend(price, Start);

//            return false;
//        }

//        private decimal SetStartPoint(decimal price)
//        {
//            //first update ever
//            if (LastPrice == 0)
//            {
//                LastPrice = price;
//                return Start;
//            }

//            //2nd update - reset start to previous tick
//            Start = LastPrice;
//            Trend = CheckTrend(price, LastPrice);
//            return Start;
//        }

//        private bool CheckTrend(decimal price, decimal refPrice)
//        {
//            var margin = refPrice * ThresholdLeg;
//            var lowBound = refPrice - ThresholdLeg;
//            var upBound = refPrice + ThresholdLeg;

//            // price is lower than higher bound - Trend is UP
//            if (price > upBound)
//            {
//                Trend = true;
//            }
//            // price is lower than lower bound - Trend is DOWN    
//            else if (price < upBound)
//            {
//                Trend = false;
//            }
//            return Trend;
//        }
//    }
//}
