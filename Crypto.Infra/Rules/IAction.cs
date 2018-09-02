using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra.Rules
{
    public interface IAction
    {
        bool Calculate(decimal value, decimal refValue, decimal threshold);
    }
}
