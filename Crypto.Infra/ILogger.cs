using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Infra
{
    public interface ILogger
    {
        Task LogAsync(string msg, int severity = 1);
        //void Log(string msg, int severity = 1);
        void Debug(string msg);
        void Info(string msg);
        void Warning(string msg);
        void Error(string msg);

        void Email(string subject, string body);
        //void Email(string subject, string body);
    }
}
