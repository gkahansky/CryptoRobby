using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace Crypto.Core_org
{
    public static class Configuration
    {
        public static string BinanceApiKey;
        public static string BinanceApiSecret;

        public static void LoadEnvironmentVariables()
        {
            Logger.Log("--------------------------------------------");
            Logger.Log("Loading Environment Configuration...");

            BinanceApiKey = "ZoNKEKTL1gbZgzkuLmm1ZhM9E09FRSGBeAkOevCy5eU82lFsb21qL24o5ZAu6Z6U";
            BinanceApiSecret = "6hN3Bzopz17fTykWwUtXjjW4Rfhy31fmBYYXaAs1vZrVjNRt401qVQwr0STSQwWS";

            Logger.Log("Environment Configuration Received Successfully");
            Logger.Log("--------------------------------------------");
        }


    }
}
