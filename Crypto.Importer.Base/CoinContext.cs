using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CryptoRobert.Infra;
using System.Data.SqlClient;
namespace CryptoRobert.Importer.Cmc
{
    public class CoinContext : DbContext
    {
        DbSet<CoinCmc> CmcCoins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //optionsBuilder.UseSqlServer("Data Source=KAHANSKY;Initial Catalog=Crypto;User Id=CryptoAdmin;Password=CryptoAdmin; Integrated Security=False");
            optionsBuilder.UseSqlServer     ("Data Source=Kahansky;Initial Catalog=Crypto;Persist Security Info=True;User ID=CryptoAdmin;Password=CryptoAdmin");
            //optionsBuilder.UseSqlServer   ("Data Source=Kahansky;Initial Catalog=Crypto;Integrated Security=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
