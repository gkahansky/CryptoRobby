using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra
{
    public interface ILogger
    {
        Task LogAsync(string msg, int severity = 1);
        void Log(string msg, int severity = 1);
        void Email(string subject, string body);
        //void Email(string subject, string body);
    }
}
