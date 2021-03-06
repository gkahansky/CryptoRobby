﻿// <auto-generated />
using CryptoRobert.Infra;
using CryptoRobert.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CryptoRobert.Infra.Data.Migrations
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

                    b.Property<string>("Symbol");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.ToTable("CoinPairs");
                });

            modelBuilder.Entity("Crypto.Infra.GlobalMarketData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveAssets");

                    b.Property<int>("ActiveCurrencies");

                    b.Property<int>("ActiveMarkets");

                    b.Property<decimal>("BitcoinDominancePct");

                    b.Property<decimal>("MarketDataUsd");

                    b.Property<decimal>("Volume24Hours");

                    b.HasKey("Id");

                    b.ToTable("MarketData");
                });

            modelBuilder.Entity("Crypto.Infra.Kline", b =>
                {
                    b.Property<string>("Symbol");

                    b.Property<int>("Interval");

                    b.Property<long>("OpenTime");

                    b.Property<decimal>("Close");

                    b.Property<long>("CloseTime");

                    b.Property<decimal>("High");

                    b.Property<decimal>("Low");

                    b.Property<decimal>("Open");

                    b.Property<decimal>("Volume");

                    b.HasKey("Symbol", "Interval", "OpenTime");

                    b.ToTable("Klines");
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
#pragma warning restore 612, 618
        }
    }
}
