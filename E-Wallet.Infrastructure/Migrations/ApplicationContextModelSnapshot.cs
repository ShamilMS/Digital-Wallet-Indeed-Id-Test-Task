﻿// <auto-generated />
using System;
using EWallet.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EWallet.Persistence.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("EWallet.Core.Models.Domain.Account", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Balance")
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("CurrencyIsoNumberCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WalletId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyIsoNumberCode");

                    b.HasIndex("WalletId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("EWallet.Core.Models.Domain.Currency", b =>
                {
                    b.Property<ushort>("IsoNumberCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IsoAlfaCode")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(3);

                    b.HasKey("IsoNumberCode");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("EWallet.Core.Models.Domain.Operation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AccountId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Direction")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("EWallet.Core.Models.Domain.Permission", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("EWallet.Core.Models.Domain.PermissionClaim", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Allowed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Permission")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(20);

                    b.Property<string>("PermissionId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.ToTable("PermissionClaims");
                });

            modelBuilder.Entity("EWallet.Core.Models.Domain.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EWallet.Core.Models.Domain.Wallet", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("EWallet.Core.Models.Domain.Account", b =>
                {
                    b.HasOne("EWallet.Core.Models.Domain.Currency", "Currency")
                        .WithMany("Accounts")
                        .HasForeignKey("CurrencyIsoNumberCode")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EWallet.Core.Models.Domain.Wallet", "Wallet")
                        .WithMany("Accounts")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EWallet.Core.Models.Domain.Operation", b =>
                {
                    b.HasOne("EWallet.Core.Models.Domain.Account", "Account")
                        .WithMany("Operations")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EWallet.Core.Models.Domain.PermissionClaim", b =>
                {
                    b.HasOne("EWallet.Core.Models.Domain.Permission", "Permission")
                        .WithMany("Claims")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EWallet.Core.Models.Domain.Wallet", b =>
                {
                    b.HasOne("EWallet.Core.Models.Domain.User", "User")
                        .WithOne("Wallet")
                        .HasForeignKey("EWallet.Core.Models.Domain.Wallet", "UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
