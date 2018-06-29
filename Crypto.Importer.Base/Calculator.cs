using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Importer.Base
{
    public static class Calculator
    {
        public static void CheckOrderValidity()
        {
            throw new NotImplementedException();
        }
                
        public static void ConvertUsdToCoinValue()
        {
            throw new NotImplementedException();
            //Get BTCUSD rate

            //Get BTCXXX rate

            //Calculate XXXUSD value

            //
        }
                
        public static void CheckFunds()
        {
            throw new NotImplementedException();
        }
                
        public static bool CheckIfUpdateRequired(long now, long last, long gap)
        {
            bool res = false;

            var diff = now - last;
            if (diff > gap)
                res = true;

            return res;
        }

        public static long CalculateEpochNow()
        {
            var now = DateTime.Now;
            long epoch = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            epoch *= 1000;

            return epoch;
        }
    }
}
