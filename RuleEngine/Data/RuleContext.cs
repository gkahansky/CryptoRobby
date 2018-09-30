using System;
using System.Collections.Generic;
using System.Text;
using CryptoRobert.RuleEngine.Entities;
using CryptoRobert.RuleEngine.Entities.MetaData;
using CryptoRobert.RuleEngine.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CryptoRobert.RuleEngine.Data
{
    public class RuleContext : DbContext
    {
        public DbSet<RuleDefinition> RuleDefinitions { get; set; }
        public DbSet<RuleSetDefinition> RuleSetDefinitions { get; set; }
        public DbSet<RuleSet> RuleSets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Crypto;User Id=CryptoAdmin;Password=CryptoAdmin");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RuleSetDefinition>()
                .HasKey(k => new { k.Id, k.RuleId });

            modelBuilder.Entity<RuleDefinition>().Ignore(r => r.Key);

            modelBuilder.Entity<RuleSet>().Ignore(s => s.Rules);
            modelBuilder.Entity<RuleSet>().Ignore(s => s.StopLoss);
            //modelBuilder.Entity<RuleSet>().Ignore(t => t.Rules);
        }
        
        //    modelBuilder.Entity<RuleBase>().Ignore(p => p.Klines);
        //}
    }
}
