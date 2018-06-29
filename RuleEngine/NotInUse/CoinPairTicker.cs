using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;

namespace CryptoRobert.RuleEngine
{
    public class CoinPairTicker
    {
        private ILogger _logger;

        private CoinPair pair;
        public CoinPair Pair
        {
            get { return pair; }
            set
            {
                pair = value;
                OnPriceChange();
            }
        }

        public delegate void PriceChangeEventHandler(object sender, EventArgs args);

        public event PriceChangeEventHandler PriceChange;

        protected virtual void OnPriceChange()
        {
            if (PriceChange != null)
                PriceChange(this, EventArgs.Empty);
        }


        public CoinPairTicker(ILogger logger)
        {
            _logger = logger;
        }

        public void UpdateTicker(decimal price)
        {
            this.pair.AvgPrice = price;
            OnPriceChange();
        }

    }
}
