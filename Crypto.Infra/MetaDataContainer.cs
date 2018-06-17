using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra
{
    public static class MetaDataContainer
    {
        public static event EventHandler klinePush;
        public static Queue<List<Kline>> KlineQueue { get; set; }

        public static void KlinePush()
        {
            if (klinePush != null)
                klinePush(KlineQueue, EventArgs.Empty);
        }
    }
}
