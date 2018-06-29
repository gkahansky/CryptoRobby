using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Infra
{
    public class User
    {
        #region Memebers

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string BinanceAPI { get; set; }
        public string BinanceSecret { get; set; }
        //public List<Wallet> Wallets { get; set; }
        public EnumCollection.UserRole UserRole { get; set; }

        #endregion

    }
}
