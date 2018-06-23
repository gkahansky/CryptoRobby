using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Crypto.Infra
{
    public class Wallet
    {
        public EnumCollection.Exchange Exchange { get; set; }
        public string Symbol { get; set; }
        public decimal QuantityFree { get; set; }
        public decimal QuantityLocked { get; set; }
        public int UserId { get; set; }
    }
}
