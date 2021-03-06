﻿// <auto-generated />
using CryptoRobert.Importer.Cmc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CryptoRobert.Importer.Cmc.Migrations
{
    [DbContext(typeof(CoinContext))]
    [Migration("20180428204536_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("CmcCoins");
                });
#pragma warning restore 612, 618
        }
    }
}
