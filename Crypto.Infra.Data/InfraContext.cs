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
        public DbSet<Kline> Klines { get; set; }
        public DbSet<CoinCmc> CmcCoins { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Crypto;User Id=CryptoAdmin;Password=CryptoAdmin");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kline>()
                .HasKey(k => new { k.Symbol, k.Interval, k.OpenTime });

            
        }
    }
}
