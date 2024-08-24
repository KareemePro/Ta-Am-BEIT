using Azure.Core;
using Firebase.Storage;
using FoodDelivery.Data;
using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DominModels.Orders;
using FoodDelivery.Models.DominModels.Subscriptions;
using FoodDelivery.Models.DTO.PromoCodeDTO;
using FoodDelivery.Models.Utility;
using FoodDelivery.Services.Common;
using Mailjet.Client.Resources;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Firebase.Auth;
using System.Linq;

namespace FoodDelivery.Services.PromoCodeService;

public class PromoCodeService : IPromoCodeService
{
    private readonly DBContext _context;
    public PromoCodeService(DBContext context)
    {
        _context = context;
    }

    public async Task<SingleResult<bool>> AddPromoCode(UpsertPromoCodeRequest request)
    {
        if (await _context.PromoCode.AnyAsync(x => x.ID == request.ID))
            return SingleResult<bool>.Failure(["This promo code already exist"], HttpStatusCode.Conflict);
        var promoCode = new PromoCode()
        {
            ID = request.ID,
            Name = request.Name,
            CreateDate = request.CreateDate,
            ExpireDate = request.ExpireDate,
            Percentage = request.Percentage,
            MaxDiscount = request.MaxDiscount
        };

        await _context.PromoCode.AddAsync(promoCode);
        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }
    public async Task<SingleResult<bool>> EditPromoCode(UpsertPromoCodeRequest request)
    {
        var promoCode = await _context.PromoCode.AsTracking().FirstOrDefaultAsync(x => x.ID == request.ID);
        if (promoCode == null)
            return SingleResult<bool>.Failure(["This promo code do not exist"], HttpStatusCode.NotFound);

        promoCode.ID = request.ID;
        promoCode.Name = request.Name;
        promoCode.CreateDate = request.CreateDate;
        promoCode.ExpireDate = request.ExpireDate;
        promoCode.Percentage = request.Percentage;
        promoCode.MaxDiscount = request.MaxDiscount;

        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }

    public async Task<DiscountData> GetPromoCodeDiscount(string promoCodeID, string CustomerID)
    {
        DiscountData discountData = await _context.CustomerPromoCode.Where(x => x.PromoCodeID == promoCodeID && x.CustomerID == CustomerID).Select(z => new DiscountData { DiscountPercentage = z.PromoCode.Percentage, MaxDiscount = (int)z.PromoCode.MaxDiscount }).FirstOrDefaultAsync();

        return discountData;
    }
    public async Task<SingleResult<bool>> AddCustomersPromoCodes(CreateCustomersPromoCodes request)
    {
        if (!await _context.PromoCode.AnyAsync(x => x.ID == request.PromoCodeID))
            return SingleResult<bool>.Failure(["This promo code does not exist"], HttpStatusCode.NotFound);

        if (!request.CustomersID.All(y => _context.Customers.Any(z => z.Id == y)))
            return SingleResult<bool>.Failure(["One or more customer do not exist"], HttpStatusCode.NotFound);

        if (await _context.CustomerPromoCode.AnyAsync(x => request.CustomersID.Contains(x.CustomerID) && x.PromoCodeID == request.PromoCodeID))
            return SingleResult<bool>.Failure(["One or more customer already has this promo code"], HttpStatusCode.Conflict);

        var customerPromoCode = new List<CustomerPromoCode>();

        foreach (var customerId in request.CustomersID)
        {
            customerPromoCode.Add(new CustomerPromoCode()
            {
                PromoCodeID = request.PromoCodeID,
                CustomerID = customerId,
                IsUsed = false,
            });
        }

        await _context.CustomerPromoCode.AddRangeAsync(customerPromoCode);
        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }

    public async Task<SingleResult<Subscription>> AddPromoCodeToSubscription(Subscription subscription)
    {
        var promocode = await _context.CustomerPromoCode.AsTracking().Include(x => x.PromoCode).Where(x => x.CustomerID == subscription.CustomerID && x.PromoCodeID == subscription.PromoCodeID).FirstOrDefaultAsync();

        if (promocode == null) return SingleResult<Subscription>.Failure(["You do not have this promo code"], HttpStatusCode.NotFound);


        if (promocode.IsUsed && subscription.SubscriptionStatus != SubscriptionStatus.Pending) return SingleResult<Subscription>.Failure(["This promo code is already used"], HttpStatusCode.Conflict);

        subscription.PromoCodeID = subscription.PromoCodeID;
        subscription.DiscountPercentage = promocode.PromoCode.Percentage;
        subscription.MaxDiscount = promocode.PromoCode.MaxDiscount;
        promocode.IsUsed = true;
        promocode.IsUsedByOrder = false;
        promocode.UsedDate = DateTime.UtcNow;

        return SingleResult<Subscription>.Success(subscription);

    }
    
    public async Task<SingleResult<Order>> AddPromoCodeToOrder(Order order)
    {
        var promocode = await _context.CustomerPromoCode.AsTracking().Include(x => x.PromoCode).Where(x => x.CustomerID == order.CustomerID && x.PromoCodeID == order.PromoCodeID).FirstOrDefaultAsync();

        if (promocode == null) return SingleResult<Order>.Failure(["You do not have this promo code"], HttpStatusCode.NotFound);


        if (promocode.IsUsed) return SingleResult<Order>.Failure(["This promo code is already used"], HttpStatusCode.Conflict);

        order.PromoCodeID = order.PromoCodeID;
        promocode.IsUsed = true;
        promocode.IsUsedByOrder = false;
        promocode.UsedDate = DateTime.UtcNow;
        order.DiscountPercentage = promocode.PromoCode.Percentage;
        order.MaxDiscount = promocode.PromoCode.MaxDiscount;

        return SingleResult<Order>.Success(order);

    }

    public async Task<SingleResult<float>> CalculateDiscount(string CustomerID, string PromoCodeID)
    {
        //var mealOptionIDss = await _context.CartItems
        //    .Where(x => x.UserID == CustomerID)
        //    .GroupBy(x => x.MealOptionID)
        //    .Select(g => new
        //    {
        //        MealOptionID = g.Key,
        //        Quantity = g.Sum(x => x.Quantity),
        //        g.First().MealOption.Price
        //    })
        //    .ToListAsync();

        //var mealOptiontotal = await _context.CartItems
        //    .Where(x => x.UserID == CustomerID)
        //    .GroupBy(x => x.MealOptionID)
        //    .Select(group => new
        //    {
        //        Quantity = group.Sum(x => x.Quantity),
        //        group.First().MealOption.Price
        //    })
        //    .ToListAsync();

        //var totalPrice = total.Sum(item => item.Quantity * item.Price);


        // Retrieve necessary data from the database
        var mealOptionData = await _context.CartItems
            .Where(x => x.UserID == CustomerID)
            .GroupBy(x => x.MealOptionID)
            .Select(group => new
            {
                Quantity = group.Sum(x => x.Quantity),
                Price = group.First().MealOption.Price
            })
            .ToListAsync();

        // Calculate total for meal options
        var mealOptionTotal = mealOptionData.Sum(item => item.Quantity * item.Price);

        // Retrieve necessary data from the database
        var sideDishData = await _context.CartItems
            .Where(x => x.UserID == CustomerID)
            .SelectMany(x => x.ItemOptions)
            .Where(z => z.MealSideDish.IsFree == false)
            .GroupBy(x => new { x.MealSideDishOptionID, x.SideDishSizeOption })
            .Select(group => new
            {
                Quantity = group.Select(x => x.CartItem.Quantity).Sum(),
                Price = group.First().SideDishOption.Price,
            })
            .ToListAsync();

        // Calculate total for side dishes
        var sideDishTotal = sideDishData.Sum(item => item.Quantity * item.Price);





        //var SideDishesIDss = await _context.CartItems
        //    .Where(x => x.UserID == CustomerID)
        //    .SelectMany(x => x.ItemOptions)
        //    .GroupBy(x => new { x.MealSideDishOptionID, x.SideDishSizeOption })
        //    .Select(x => new
        //    {
        //        SideDishID = x.Key.MealSideDishOptionID,
        //        x.Key.SideDishSizeOption,
        //        x.First().SideDishOption.Price,
        //        Quantity = x.Count()
        //    })
        //    .ToListAsync();


        //var mealOptionIDs = request.MealData.Select(x => x.MealOptionID);
        //var ValidMealOptionsCount = await _context.MealOptions.CountAsync(x => mealOptionIDs.Contains(x.ID) && x.IsAvailable);

        //if (ValidMealOptionsCount != request.MealData.Count)
        //    return SingleResult<float>.Failure([("please recheck you cart meals")], HttpStatusCode.NotFound);

        //var mealOptionPrices = await _context.MealOptions
        //    .Where(x => mealOptionIDs.Contains(x.ID))
        //    .ToDictionaryAsync(x => x.ID, x => x.Price);

        //// Get a distinct list of side dish IDs and size options from the request
        //var sideDishIds = request.SideDishData.Select(x => x.SideDishID).Distinct().ToList();
        //var sizeOptions = request.SideDishData.Select(x => x.SideDishSizeOption).Distinct().ToList();

        //// Retrieve all matching side dish options from the database
        //var sideDishOptions = await _context.SideDishOptions
        //    .Where(s => sideDishIds.Contains(s.SideDishID) && sizeOptions.Contains(s.SideDishSizeOption))
        //    .ToListAsync();

        // Calculate total prices for each item based on the retrieved side dish options
        //float sideDishPrices = 0;

        //foreach (var sideDishData in request.SideDishData)
        //{
        //    var sideDishOption = sideDishOptions.FirstOrDefault(s =>
        //        s.SideDishID == sideDishData.SideDishID && s.SideDishSizeOption == sideDishData.SideDishSizeOption);

        //    if (sideDishOption != null)
        //    {
        //        sideDishPrices += sideDishOption.Price * sideDishData.Quantities;
        //    }
        //}

        // sideDishPrices now contains the total price for each item separately





        //error
        //var sideDishOptionIDs = await _context.Cart.Where(c => c.UserID == request.CustomerID).AsTracking().Select(x => new
        //{
        //    SideDishID = x.SelectedSideDishes.Select(y => y.SideDishID),
        //    SideDishSizeOption = x.SelectedSideDishes.Select(y => y.SideDishSizeOption),
        //}).ToListAsync();


        //var sideDishesPrices = await _context.Cart
        //    .Where(x => x.UserID == request.CustomerID)
        //    .Select(f => f.MealOption)
        //    .Where(a => mealOptionIDs.Contains(a.ID))
        //    .SelectMany(l => l.MealSideDishes)
        //    .Where(k => !k.IsFree)
        //    .SelectMany(b => b.MealSideDishOptions)
        //    .Where(m => sideDishOptionIDs.SelectMany(x => x.SideDishID).Contains(m.SideDishID) && sideDishOptionIDs.SelectMany(x => x.SideDishSizeOption).Contains(m.SideDishSizeOption))
        //    .Select(z => new { z.MealSideDish.MealOptionID, z.SideDishID, z.SideDishSizeOption, z.SideDishOption.Price })
        //    .ToListAsync();

        var discountData = await GetPromoCodeDiscount(PromoCodeID, CustomerID);
        if(discountData == null)
            return SingleResult<float>.Failure([("Wrong promo code")], HttpStatusCode.NotFound);

        double TotalAmount = sideDishTotal + mealOptionTotal + 20;//mealOptionPrices.Sum(kv => kv.Value * request.MealData.First(item => item.MealOptionID == kv.Key).Quantities) + sideDishPrices + 20;//error sideDishesPrices.Sum(x => x.Price);

        var MaxDiscount = (float)Math.Max(TotalAmount - discountData.MaxDiscount, TotalAmount * (1 - (float)discountData.DiscountPercentage));

        return SingleResult<float>.Success(MaxDiscount);

    }

}
