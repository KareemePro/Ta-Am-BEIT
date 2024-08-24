using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DominModels.Address;
using FoodDelivery.Models.DominModels.Auth;
using FoodDelivery.Models.DominModels.Meals;
using FoodDelivery.Models.DominModels.Orders;
using FoodDelivery.Models.DominModels.Subscriptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;

namespace FoodDelivery.Data;

public class DBContext : IdentityDbContext<User>
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Chief>(entity => { entity.ToTable("Chiefs"); });
        builder.Entity<Customer>(entity => { entity.ToTable("Customers"); });

        //builder.Entity<Models.DominModels.MealTag>()
        //    .HasKey(e => new { e.TagID, e.MealID });



        builder.Entity<SelectedSideDish>()
            .HasKey(e => new { e.UserID, e.MealOptionID, e.SideDishID });

        builder.Entity<Cart>()
            .HasMany(x => x.Items)
            .WithOne(x => x.Cart)
            .HasForeignKey(x => new { x.UserID });

        builder.Entity<CartItem>()
            .HasMany(x => x.ItemOptions)
            .WithOne(x => x.CartItem)
            .HasForeignKey(x => new { x.CartItemID });

        builder.Entity<SelectedSideDish>()
            .HasOne(x => x.SideDishOption)
            .WithMany()
            .HasForeignKey(x => new { x.SideDishID, x.SideDishSizeOption });

        builder.Entity<MealOption>()
            .HasAlternateKey(e => new { e.MealSizeOption, e.MealID });

        builder.Entity<MealOption>()
            .HasMany(e => e.MealOptionIngredients)
            .WithOne(x => x.MealOption)
            .HasForeignKey(x => x.MealOptionID);

        builder.Entity<ChiefIngredient>()
            .HasMany(e => e.MealOptionIngredients)
            .WithOne(x => x.ChiefIngredient)
            .HasForeignKey(x => new { x.ChiefID, x.FoodIngredient });

        builder.Entity<ChiefIngredient>()
            .HasKey(e => new { e.ChiefID, e.FoodIngredient });


        builder.Entity<MealOptionIngredient>()
            .HasKey(e => new { e.FoodIngredient, e.ChiefID, e.MealOptionID });


        builder.Entity<WishList>()
            .HasKey(e => new { e.UserID, e.MealOptionID });

        builder.Entity<OrderItemOption>()
            .HasOne(r => r.SideDishOption)
            .WithMany()
            .HasForeignKey(e => new { e.SideDishID, e.SideDishSizeOption });

        //builder.Entity<Tag>()
        //    .HasMany(e => e.MealTags)
        //    .WithOne(e => e.Tag)
        //    .HasForeignKey(e => e.TagID);

        builder.Entity<Meal>()
            .HasMany(e => e.MealReviews)
            .WithOne(e => e.Meal);
        builder.Entity<Customer>()
            .HasMany(e => e.MealReviews)
            .WithOne(e => e.Customer);
        builder.Entity<MealReview>()
            .HasKey(e => new { e.MealID, e.CustomerID })
            .IsClustered(clustered: false);

        builder.Entity<MealReview>()
            .HasIndex(e => e.MealID)
            .IsClustered();


        builder.Entity<Models.DominModels.Meals.MealTag>()
            .HasKey(e => new { e.MealID, e.Tag });
        //builder.Entity<MealReview>()
        //    .Property(x => x.Rating)
        //    .HasPrecision(3, 2);

        //builder.Entity<Meal>()
        //    .Property(x => x.Rating)
        //    .HasPrecision(3, 2);

        builder.Entity<MealOption>()
            .HasIndex(x => x.Price);

        //builder.Entity<PromoCode>()
        //    .Property(x => x.Percentage)
        //    .HasPrecision(2, 2);

        //builder.Entity<MealSideDishOption>()
        //    .HasKey(fsd => new { fsd.SideDishID, fsd.MealOptionID });

        //builder.Entity<MealOption>()
        //    .HasMany(fsd => fsd.FreeSideDishOptions)
        //    .WithOne(mo => mo.MealOption)
        //    .HasForeignKey(fsd => fsd.MealOptionID);

        //builder.Entity<MealOption>()
        //    .HasMany(fsd => fsd.FreeSideDishOptions)
        //    .WithOne(mo => mo.MealOption)
        //    .HasForeignKey(fsd => fsd.MealOptionID);

        builder.Entity<Chief>()
            .HasOne(e => e.ChiefReview)
            .WithOne(e => e.Chief);
        builder.Entity<Customer>()
            .HasOne(e => e.ChiefReview)
            .WithOne(e => e.Customer);
        builder.Entity<ChiefReview>()
            .HasKey(e => new { e.ChiefID, e.CustomerID });
        builder.Entity<SubscriptionDayData>()
            .HasKey(e => new { e.SubscriptionID, e.MealOptionID, e.DeliveryDate });
        //builder.Entity<OrderItem>()
        //    .HasKey(e => new { e.OrderID, e.MealOptionID });
        builder.Entity<CustomerPromoCode>()
            .HasKey(e => new { e.PromoCodeID, e.CustomerID });
        builder.Entity<SideDishOption>()
            .HasKey(e => new { e.SideDishID, e.SideDishSizeOption });
        builder.Entity<CustomerPromoCode>()
            .HasOne(e => e.Order)
            .WithOne(e => e.CustomerPromoCode)
            .HasForeignKey<Order>(e => new { e.PromoCodeID, e.CustomerID } );

        builder.Entity<Cart>()
            .HasKey(e => new { e.UserID });
        builder.Entity<Cart>()
            .HasOne(e => e.User)
            .WithOne();
        builder.Entity<CartItem>()
            .Property(e => e.ID)
            .ValueGeneratedOnAdd();
        builder.Entity<CartItemOption>()
            .Property(e => e.ID)
            .ValueGeneratedOnAdd();

        builder.Entity<CartItemOption>()
            .HasIndex(e => new { e.CartItemID, e.MealSideDishID })
            .IsUnique();
    }
    public DbSet<MealTag> MealTags { get; set; }
    public DbSet<Chief> Chiefs { get; set; }

    public DbSet<Meal> Meals { get; set; }

    //public DbSet<Tag> Tags { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Governorate> Governorates { get; set; }

    public DbSet<District> Districts { get; set; }

    public DbSet<Street> Streets { get; set; }

    public DbSet<Building> Buildings { get; set; }

    public DbSet<MealReview> MealReviews { get; set; }

    //public DbSet<SizeOption> SizeOptions { get; set; }

    public DbSet<MealOption> MealOptions { get; set; }

    public DbSet<ChiefIngredient> ChiefIngredients { get; set; }

    public DbSet<MealOptionIngredient> MealOptionIngredients { get; set; }

    public DbSet<MealSideDish> MealSideDishes { get; set; }

    public DbSet<SideDish> SideDishes { get; set; }

    public DbSet<SideDishOption> SideDishOptions { get; set; }

    public DbSet<WishList> WishLists { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<OrderItemOption> OrderItemOptions { get; set; }

    public DbSet<Subscription> Subscriptions { get; set; }

    public DbSet<SubscriptionDayData> SubscriptionsDaysData { get; set; }

    public DbSet<PromoCode> PromoCode { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<CustomerPromoCode> CustomerPromoCode { get; set; }

    public DbSet<Cart> Cart { get; set; }
}
