﻿// <auto-generated />
using System;
using STS.Infrastructure.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace STS.Infrastructure.Localization.Migrations
{
    [DbContext(typeof(LocalizationDbContext))]
    partial class LocalizationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("STS.Domain.Entities.Localization.Culture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cultures");
                });

            modelBuilder.Entity("STS.Domain.Entities.Localization.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CultureId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CultureId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("STS.Domain.Entities.Localization.Resource", b =>
                {
                    b.HasOne("STS.Domain.Entities.Localization.Culture", "Culture")
                        .WithMany("Resources")
                        .HasForeignKey("CultureId");
                });
#pragma warning restore 612, 618
        }
    }
}
