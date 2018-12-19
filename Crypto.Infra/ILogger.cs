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
        Task Debug(string msg);
        Task Info(string msg);
        Task Warning(string msg);
        Task Error(string msg);
        void Stats(string msg);
        void InitializeStatsReport();

        void Email(string subject, string body);
        //void Email(string subject, string body);
    }
}
