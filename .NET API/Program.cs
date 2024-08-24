using FoodDelivery.Data;
using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Services.Addresses;
using FoodDelivery.Services.Admin;
using FoodDelivery.Services.Auth;
using FoodDelivery.Services.CartService;
using FoodDelivery.Services.Cheifs;
using FoodDelivery.Services.Common;
using FoodDelivery.Services.Emails;
using FoodDelivery.Services.MealReviews;
using FoodDelivery.Services.Meals;
using FoodDelivery.Services.Orders;
using FoodDelivery.Services.Payment;
using FoodDelivery.Services.PromoCodeService;
using FoodDelivery.Services.SideDishes;
using FoodDelivery.Services.Subscriptions;
using FoodDelivery.Services.WishLists;
using FoodDelivery.Settings;
using Mailjet.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using X.Paymob.CashIn;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<DBContext>();


builder.Services.AddIdentityCore<Customer>().AddRoles<IdentityRole>().AddEntityFrameworkStores<DBContext>();

builder.Services.AddIdentityCore<Chief>().AddRoles<IdentityRole>().AddEntityFrameworkStores<DBContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.AllowedForNewUsers = false;
});

builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MonsterConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IMealService, MealService>();

builder.Services.AddTransient<IImageService, ImageService>();

builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddScoped<IMealReviewService, MealReviewService>();

builder.Services.AddScoped<ISubscriptionServices, SubscriptionServices>();

builder.Services.AddScoped<IPromoCodeService, PromoCodeService>(); 

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IWishListService, WishListService>();

builder.Services.AddScoped<ISideDishService, SideDishService>();

builder.Services.AddScoped<IChiefService, ChiefService>();

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddPaymobCashIn(config => {
    config.ApiKey = builder.Configuration["Paymob:APIKey"];
    config.Hmac = builder.Configuration["Paymob:HMAC"];
});

builder.Services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
{
    client.SetDefaultSettings();
    client.UseBasicAuthentication(builder.Configuration["Mailjet:PublicKey"], builder.Configuration["Mailjet:SecretKey"]);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddSignalR();
builder.Services.AddCors();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    //options.JsonSerializerOptions.Converters.Add(new EnumConverter());
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//var someService = app.Services.GetService(ImageService );


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           builder.Environment.WebRootPath)
}); 
app.UseAuthentication();
app.UseAuthorization();
app.UseDeveloperExceptionPage();

app.UseCors(x => x
.SetIsOriginAllowed(origin => true)
.AllowAnyMethod()
.AllowAnyHeader()
.WithExposedHeaders("Location")
.AllowCredentials());

app.MapControllers();

app.Run();
