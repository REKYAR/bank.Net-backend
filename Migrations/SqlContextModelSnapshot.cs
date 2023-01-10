﻿// <auto-generated />
using System;
using Bank.NET___backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bank.NETbackend.Migrations
{
    [DbContext(typeof(SqlContext))]
    partial class SqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Bank.NET___backend.ApiStructures.NewFolder.bankapi4dotnet.Inquiry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DocumentId")
                        .HasColumnType("integer");

                    b.Property<int>("DocumentType")
                        .HasColumnType("integer");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("IncomeLevel")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("InstallmentsCount")
                        .HasColumnType("integer");

                    b.Property<int>("JobType")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("MoneyAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Inquiries");
                });

            modelBuilder.Entity("Bank.NET___backend.Data.Admin", b =>
                {
                    b.Property<int>("AdminID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AdminID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AdminID");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Bank.NET___backend.Data.Request", b =>
                {
                    b.Property<int>("RequestID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RequestID"));

                    b.Property<string>("AgreementKey")
                        .HasColumnType("text");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("ApiInfo")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DocumentKey")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("External")
                        .HasColumnType("boolean");

                    b.Property<string>("GovermentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("IncomeLevel")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("InquireId")
                        .HasColumnType("uuid");

                    b.Property<string>("JobType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("MappedGuid")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumberOfInstallments")
                        .HasColumnType("integer");

                    b.Property<int?>("ResponseID")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("UserID")
                        .HasColumnType("integer");

                    b.HasKey("RequestID");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Bank.NET___backend.Data.Response", b =>
                {
                    b.Property<int>("ResponseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ResponseID"));

                    b.Property<string>("ApiInfo")
                        .HasColumnType("text");

                    b.Property<bool>("External")
                        .HasColumnType("boolean");

                    b.Property<decimal>("MonthlyInstallment")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("OfferId")
                        .HasColumnType("uuid");

                    b.Property<int>("RequestID")
                        .HasColumnType("integer");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("documentKey")
                        .HasColumnType("text");

                    b.HasKey("ResponseID");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("Bank.NET___backend.Data.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("GovermentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("IncomeLevel")
                        .HasColumnType("numeric");

                    b.Property<string>("JobType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
