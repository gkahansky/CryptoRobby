using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Infra
{
    public class DataRepository
    {
        public Queue<Kline> Klines;
        

        public DataRepository()
        {
            Klines = new Queue<Kline>();
        }
    }
}
