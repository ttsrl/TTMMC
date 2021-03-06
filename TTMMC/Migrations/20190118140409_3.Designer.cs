﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TTMMC.Services;

namespace TTMMC.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20190118140409_3")]
    partial class _3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TTMMC.Models.DBModels.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("Code");

                    b.Property<string>("Email");

                    b.Property<string>("FiscalCode");

                    b.Property<string>("Name");

                    b.Property<string>("PEC");

                    b.Property<string>("Phone");

                    b.Property<string>("Province");

                    b.Property<string>("State");

                    b.Property<string>("Town");

                    b.Property<string>("VAT");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("TTMMC.Models.DBModels.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("TTMMC.Models.DBModels.Mould", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClientId");

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Location");

                    b.Property<string>("Notes");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Moulds");
                });

            modelBuilder.Entity("TTMMC.Models.DBModels.Mould", b =>
                {
                    b.HasOne("TTMMC.Models.DBModels.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");
                });
#pragma warning restore 612, 618
        }
    }
}
