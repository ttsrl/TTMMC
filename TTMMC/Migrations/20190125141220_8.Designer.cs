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
    [Migration("20190125141220_8")]
    partial class _8
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

            modelBuilder.Entity("TTMMC.Models.DBModels.Layout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Barcode");

                    b.Property<int?>("ClientId");

                    b.Property<string>("Description");

                    b.Property<int>("Machine");

                    b.Property<int?>("MasterId");

                    b.Property<string>("Minced");

                    b.Property<int?>("MixtureId");

                    b.Property<int?>("MouldId");

                    b.Property<int>("Packaging");

                    b.Property<int>("PackagingQuantity");

                    b.Property<int>("Quantity");

                    b.Property<DateTime>("Start");

                    b.Property<int>("Status");

                    b.Property<TimeSpan>("Umidification");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("MasterId");

                    b.HasIndex("MixtureId");

                    b.HasIndex("MouldId");

                    b.ToTable("Layouts");
                });

            modelBuilder.Entity("TTMMC.Models.DBModels.Master", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<int>("ColorArgb");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Masters");
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

            modelBuilder.Entity("TTMMC.Models.DBModels.Mixture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("Notes");

                    b.HasKey("Id");

                    b.ToTable("Mixtures");
                });

            modelBuilder.Entity("TTMMC.Models.DBModels.MixtureItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MaterialId");

                    b.Property<int?>("MixtureId");

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.HasIndex("MixtureId");

                    b.ToTable("MixtureItems");
                });

            modelBuilder.Entity("TTMMC.Models.DBModels.Mould", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<int?>("DefaultClientId");

                    b.Property<int?>("DefaultMixtureId");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Location");

                    b.Property<string>("Notes");

                    b.HasKey("Id");

                    b.HasIndex("DefaultClientId");

                    b.HasIndex("DefaultMixtureId");

                    b.ToTable("Moulds");
                });

            modelBuilder.Entity("TTMMC.Models.DBModels.Layout", b =>
                {
                    b.HasOne("TTMMC.Models.DBModels.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.HasOne("TTMMC.Models.DBModels.Master", "Master")
                        .WithMany()
                        .HasForeignKey("MasterId");

                    b.HasOne("TTMMC.Models.DBModels.Mixture", "Mixture")
                        .WithMany()
                        .HasForeignKey("MixtureId");

                    b.HasOne("TTMMC.Models.DBModels.Mould", "Mould")
                        .WithMany()
                        .HasForeignKey("MouldId");
                });

            modelBuilder.Entity("TTMMC.Models.DBModels.MixtureItem", b =>
                {
                    b.HasOne("TTMMC.Models.DBModels.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId");

                    b.HasOne("TTMMC.Models.DBModels.Mixture")
                        .WithMany("Items")
                        .HasForeignKey("MixtureId");
                });

            modelBuilder.Entity("TTMMC.Models.DBModels.Mould", b =>
                {
                    b.HasOne("TTMMC.Models.DBModels.Client", "DefaultClient")
                        .WithMany()
                        .HasForeignKey("DefaultClientId");

                    b.HasOne("TTMMC.Models.DBModels.Mixture", "DefaultMixture")
                        .WithMany()
                        .HasForeignKey("DefaultMixtureId");
                });
#pragma warning restore 612, 618
        }
    }
}
