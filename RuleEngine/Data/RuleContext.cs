using System;
using System.Collections.Generic;
using System.Text;
using Crypto.RuleEngine.Entities;
using CryptoRobert.RuleEngine.Entities;
using CryptoRobert.RuleEngine.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Crypto.RuleEngine.Data
{
    public class RuleContext : DbContext
    {
        public DbSet<RuleDefinition> RuleDefinitions { get; set; }
        public DbSet<RuleSetDefinition> RuleSetDefinitions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Crypto;User Id=CryptoAdmin;Password=CryptoAdmin");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RuleSetDefinition>()
                .HasKey(k => new { k.Id, k.RuleId });
        }
        //    modelBuilder.Entity<RuleBase>().Ignore(p => p.RuleSets);
        //    modelBuilder.Entity<RuleBase>().Ignore(p => p.Klines);
        //}
    }
}
