using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Crypto.Core_org
{
    public static class Logger
    {
        const string pathString = @"C:\Cryptik\Log\";
        public static string path = pathString + "PriceLog" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".txt";

        public static void Log(string msg, int severity = 1)
        {

            using (StreamWriter sw = File.AppendText(path))
            {
                var now = DateTime.Now.ToString("yyy-MM-dd hh:mm:ss\t");
                sw.WriteLine(now + msg);
                switch (severity)
                {
                    case 0:
                        break;
                    default:
                        Console.WriteLine(now + msg);
                        break;
                }

            }
        }
    }
}
