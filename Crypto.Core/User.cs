using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core_org
{
    public class User
    {
        #region Memebers

        int Id { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string BinanceAPI { get; set; }
        string BinanceSecret { get; set; }
        List<Wallet> Wallets { get; set; }
        EnumCollection.UserRole UserRole { get; set; }

        #endregion

    }
}
