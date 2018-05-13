using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra
{
    public interface ILogger
    {
        void Log(string msg, int severity = 1);
    }
}
