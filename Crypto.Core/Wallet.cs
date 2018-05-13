using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Enums;

namespace Crypto.Core_org
{
    public class Wallet
    {
        public int Id { get; set; }
        public EnumCollection.Exchange Exchange { get; set; }
        public int CoinId { get; set; }
        public int Quantity { get; set; }
    }
}
