﻿// <auto-generated />
using Crypto.Infra;
using Crypto.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Crypto.Infra.Data.Migrations
{
    [DbContext(typeof(InfraContext))]
    partial class InfraContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Crypto.Infra.Coin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Symbol");

                    b.HasKey("Id");

                    b.ToTable("Coins");
                });

            modelBuilder.Entity("Crypto.Infra.CoinCmc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AvailableSupply");

                    b.Property<decimal>("ChangePct1Hr");

                    b.Property<decimal>("ChangePct24Hr");

                    b.Property<decimal>("ChangePct7d");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<decimal>("MarketCapUsd");

                    b.Property<decimal>("MaxSupply");

                    b.Property<string>("Name");

                    b.Property<decimal>("PriceBtc");

                    b.Property<decimal>("PriceUsd");

                    b.Property<int>("Rank");

                    b.Property<string>("Symbol");

                    b.Property<decimal>("TotalSupply");

                    b.Property<decimal>("VolumeUsd");

                    b.HasKey("Id");

                    b.ToTable("CoinCmcs");
                });

            modelBuilder.Entity("Crypto.Infra.CoinPair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Coin1Id");

                    b.Property<int>("Coin2Id");

                    b.Property<decimal>("Price");

                    b.Property<string>("Symbol");

                    b.HasKey("Id");

                    b.ToTable("CoinPairs");
                });

            modelBuilder.Entity("Crypto.Infra.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BinanceAPI");

                    b.Property<string>("BinanceSecret");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.Property<int>("UserRole");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Crypto.Infra.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CoinId");

                    b.Property<int>("Exchange");

                    b.Property<int>("Quantity");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("Crypto.Infra.Wallet", b =>
                {
                    b.HasOne("Crypto.Infra.User")
                        .WithMany("Wallets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
