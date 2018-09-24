using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.RuleEngine.Interfaces
{
    public interface IRuleCalculator
    {
        decimal CalculateTrend(decimal LastAvgPrice, decimal avgPrice);
        bool CheckThreshold(decimal threshold, decimal value, int op);
    }
}
