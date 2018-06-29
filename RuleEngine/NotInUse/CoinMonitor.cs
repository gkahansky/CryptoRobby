using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;

namespace CryptoRobert.RuleEngine
{
    public class CoinMonitor 
    {
        private ILogger _logger;

        public CoinMonitor(ILogger logger)
        {
            _logger = logger;
            
        }

        public void OnPriceChange(object source, EventArgs e)
        {
            _logger.Log("price changed");
        }
        

        

    }
}
