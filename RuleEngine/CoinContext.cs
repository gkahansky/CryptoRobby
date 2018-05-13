using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Crypto.Infra;

namespace Crypto.RuleEngine
{
    public class CoinContext : DbContext
    {
        DbSet<CoinCmc> CmcCoins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=Kahansky;Initial Catalog=Crypto;Integrated Security=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
