using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Crypto.Infra.Data
{
    public class InfraContext : DbContext
    {
        public DbSet<Coin> Coins { get; set; }
        public DbSet<CoinPair> CoinPairs { get; set; }
        public DbSet<CoinCmc> CoinCmcs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GlobalMarketData> MarketData { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.SqlConnectionString);
        }
    }
}
