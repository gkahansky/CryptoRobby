﻿// <auto-generated />
using CryptoRobert.RuleEngine.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CryptoRobert.RuleEngine.Migrations
{
    [DbContext(typeof(RuleContext))]
    partial class RuleContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CryptoRobert.RuleEngine.Entities.MetaData.RuleDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Interval");

                    b.Property<int>("Operator");

                    b.Property<int>("Priority");

                    b.Property<int>("Retention");

                    b.Property<string>("RuleType");

                    b.Property<bool>("State");

                    b.Property<string>("Symbol");

                    b.Property<decimal>("Threshold");

                    b.HasKey("Id");

                    b.ToTable("RuleDefinitions");
                });

            modelBuilder.Entity("CryptoRobert.RuleEngine.Entities.MetaData.RuleSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Buy");

                    b.Property<string>("Description");

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("Name");

                    b.Property<string>("PairToBuy");

                    b.Property<decimal>("Score");

                    b.Property<decimal>("Threshold");

                    b.HasKey("Id");

                    b.ToTable("RuleSets");
                });

            modelBuilder.Entity("CryptoRobert.RuleEngine.Entities.MetaData.RuleSetDefinition", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("RuleId");

                    b.HasKey("Id", "RuleId");

                    b.ToTable("RuleSetDefinitions");
                });
#pragma warning restore 612, 618
        }
    }
}
