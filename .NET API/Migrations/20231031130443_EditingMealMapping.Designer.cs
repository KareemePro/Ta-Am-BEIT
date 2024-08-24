﻿// <auto-generated />
using System;
using FoodDelivery.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FoodDelivery.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20231031130443_EditingMealMapping")]
    partial class EditingMealMapping
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Address.Building", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StreetID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("StreetID");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Address.District", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GovernorateID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("GovernorateID");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Address.Governorate", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Governorates");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Address.Street", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DistrictID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("DistrictID");

                    b.ToTable("Streets");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.ChiefReview", b =>
                {
                    b.Property<string>("ChiefID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ChiefID", "CustomerID");

                    b.HasIndex("ChiefID")
                        .IsUnique();

                    b.HasIndex("CustomerID")
                        .IsUnique();

                    b.ToTable("ChiefReview");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Meal", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ChiefID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsSideDish")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ChiefID");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.MealImage", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MealID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("MealID");

                    b.ToTable("MealImage");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.MealOption", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<Guid>("MealID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<Guid>("SizeOptionID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("MealID");

                    b.HasIndex("SizeOptionID");

                    b.ToTable("MealOptions");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.MealReview", b =>
                {
                    b.Property<Guid>("MealID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CustomerID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("ReviewImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MealID", "CustomerID");

                    b.HasIndex("CustomerID")
                        .IsUnique();

                    b.HasIndex("MealID")
                        .IsUnique();

                    b.ToTable("MealReviews");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.MealTag", b =>
                {
                    b.Property<Guid>("TagID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MealID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagID", "MealID");

                    b.HasIndex("MealID");

                    b.ToTable("MealTag");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Orders.Order", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BuildingID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CustomerID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("BuildingID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Orders.OrderItem", b =>
                {
                    b.Property<Guid>("OrderID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MealOptionID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelivered")
                        .HasColumnType("bit");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderID", "MealOptionID");

                    b.HasIndex("MealOptionID");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.SizeOption", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Name")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.ToTable("SizeOptions");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Subscriptions.Subscription", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CustomerID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.Property<double>("TotalAmount")
                        .HasColumnType("float");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Subscriptions.SubscriptionDayData", b =>
                {
                    b.Property<Guid>("SubscriptionID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MealOptionID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeliveredDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("SubscriptionID", "MealOptionID", "DeliveryDate");

                    b.HasIndex("MealOptionID");

                    b.ToTable("SubscriptionsDaysData");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Tag", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Chief", b =>
                {
                    b.HasBaseType("FoodDelivery.Models.DominModels.User");

                    b.Property<Guid>("BuildingID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("GovernmentID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasIndex("BuildingID");

                    b.ToTable("Chiefs", (string)null);
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Customer", b =>
                {
                    b.HasBaseType("FoodDelivery.Models.DominModels.User");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Address.Building", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Address.Street", "Street")
                        .WithMany("Buildings")
                        .HasForeignKey("StreetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Street");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Address.District", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Address.Governorate", "Governorate")
                        .WithMany("Districts")
                        .HasForeignKey("GovernorateID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Governorate");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Address.Street", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Address.District", "District")
                        .WithMany("Streets")
                        .HasForeignKey("DistrictID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.ChiefReview", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Chief", "Chief")
                        .WithOne("ChiefReview")
                        .HasForeignKey("FoodDelivery.Models.DominModels.ChiefReview", "ChiefID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodDelivery.Models.DominModels.Customer", "Customer")
                        .WithOne("ChiefReview")
                        .HasForeignKey("FoodDelivery.Models.DominModels.ChiefReview", "CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chief");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Meal", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Chief", "Chief")
                        .WithMany("Meals")
                        .HasForeignKey("ChiefID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chief");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.MealImage", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Meal", "Meal")
                        .WithMany("MealImages")
                        .HasForeignKey("MealID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.MealOption", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Meal", "Meal")
                        .WithMany("MealOptions")
                        .HasForeignKey("MealID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodDelivery.Models.DominModels.SizeOption", "SizeOption")
                        .WithMany("MealOptions")
                        .HasForeignKey("SizeOptionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meal");

                    b.Navigation("SizeOption");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.MealReview", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Customer", "Customer")
                        .WithOne("MealReview")
                        .HasForeignKey("FoodDelivery.Models.DominModels.MealReview", "CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodDelivery.Models.DominModels.Meal", "Meal")
                        .WithOne("MealReview")
                        .HasForeignKey("FoodDelivery.Models.DominModels.MealReview", "MealID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.MealTag", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Meal", "Meal")
                        .WithMany("MealTags")
                        .HasForeignKey("MealID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodDelivery.Models.DominModels.Tag", "Tag")
                        .WithMany("MealTags")
                        .HasForeignKey("TagID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meal");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Orders.Order", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Address.Building", "Building")
                        .WithMany()
                        .HasForeignKey("BuildingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodDelivery.Models.DominModels.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Building");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Orders.OrderItem", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.MealOption", "MealOption")
                        .WithMany("OrderItems")
                        .HasForeignKey("MealOptionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodDelivery.Models.DominModels.Orders.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MealOption");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Subscriptions.Subscription", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Subscriptions.SubscriptionDayData", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.MealOption", "MealOption")
                        .WithMany("SubscriptionsDaysData")
                        .HasForeignKey("MealOptionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodDelivery.Models.DominModels.Subscriptions.Subscription", "Subscription")
                        .WithMany("SubscriptionDayData")
                        .HasForeignKey("SubscriptionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MealOption");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.User", b =>
                {
                    b.OwnsMany("FoodDelivery.Models.DominModels.Auth.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<string>("UserId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<DateTime>("CreatedOn")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("ExpiresOn")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime?>("RevokedOn")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Token")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserId", "Id");

                            b1.ToTable("RefreshToken");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodDelivery.Models.DominModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Chief", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.Address.Building", "Building")
                        .WithMany()
                        .HasForeignKey("BuildingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodDelivery.Models.DominModels.User", null)
                        .WithOne()
                        .HasForeignKey("FoodDelivery.Models.DominModels.Chief", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Building");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Customer", b =>
                {
                    b.HasOne("FoodDelivery.Models.DominModels.User", null)
                        .WithOne()
                        .HasForeignKey("FoodDelivery.Models.DominModels.Customer", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Address.District", b =>
                {
                    b.Navigation("Streets");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Address.Governorate", b =>
                {
                    b.Navigation("Districts");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Address.Street", b =>
                {
                    b.Navigation("Buildings");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Meal", b =>
                {
                    b.Navigation("MealImages");

                    b.Navigation("MealOptions");

                    b.Navigation("MealReview")
                        .IsRequired();

                    b.Navigation("MealTags");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.MealOption", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("SubscriptionsDaysData");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Orders.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.SizeOption", b =>
                {
                    b.Navigation("MealOptions");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Subscriptions.Subscription", b =>
                {
                    b.Navigation("SubscriptionDayData");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Tag", b =>
                {
                    b.Navigation("MealTags");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Chief", b =>
                {
                    b.Navigation("ChiefReview")
                        .IsRequired();

                    b.Navigation("Meals");
                });

            modelBuilder.Entity("FoodDelivery.Models.DominModels.Customer", b =>
                {
                    b.Navigation("ChiefReview")
                        .IsRequired();

                    b.Navigation("MealReview")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
