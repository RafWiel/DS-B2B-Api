﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiService.Data;

#nullable disable

namespace WebApiService.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebApiService.Models.CompanyModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("ErpId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Postal")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("TaxNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("WebApiService.Models.CustomerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CompanyModelId")
                        .HasColumnType("int");

                    b.Property<bool>("IsMailing")
                        .HasColumnType("bit");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.Property<int?>("UserId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyModelId");

                    b.HasIndex("UserId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("WebApiService.Models.EmployeeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsMailing")
                        .HasColumnType("bit");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.Property<int?>("UserId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("WebApiService.Models.ServiceRequestModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("ClosureDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Invoice")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("Ordinal")
                        .HasColumnType("int");

                    b.Property<int?>("PartnerCompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("PrefferedEmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ReminderDate")
                        .HasColumnType("datetime2");

                    b.Property<byte>("RequestType")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<byte>("SubmitType")
                        .HasColumnType("tinyint");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PartnerCompanyId");

                    b.HasIndex("PrefferedEmployeeId");

                    b.ToTable("ServiceRequests");
                });

            modelBuilder.Entity("WebApiService.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApiService.Models.CustomerModel", b =>
                {
                    b.HasOne("WebApiService.Models.CompanyModel", null)
                        .WithMany("Customers")
                        .HasForeignKey("CompanyModelId");

                    b.HasOne("WebApiService.Models.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApiService.Models.EmployeeModel", b =>
                {
                    b.HasOne("WebApiService.Models.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApiService.Models.ServiceRequestModel", b =>
                {
                    b.HasOne("WebApiService.Models.CustomerModel", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("WebApiService.Models.EmployeeModel", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("WebApiService.Models.CompanyModel", "PartnerCompany")
                        .WithMany()
                        .HasForeignKey("PartnerCompanyId");

                    b.HasOne("WebApiService.Models.EmployeeModel", "PrefferedEmployee")
                        .WithMany()
                        .HasForeignKey("PrefferedEmployeeId");

                    b.Navigation("Customer");

                    b.Navigation("Employee");

                    b.Navigation("PartnerCompany");

                    b.Navigation("PrefferedEmployee");
                });

            modelBuilder.Entity("WebApiService.Models.CompanyModel", b =>
                {
                    b.Navigation("Customers");
                });
#pragma warning restore 612, 618
        }
    }
}
