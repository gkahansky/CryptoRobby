using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra
{
    public static class MetaDataContainer
    {
        public static Queue<List<Kline>> KlineQueue { get; set; }
    }
}
