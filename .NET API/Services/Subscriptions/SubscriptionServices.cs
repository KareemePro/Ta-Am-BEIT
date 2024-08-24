using FoodDelivery.Data;
using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Orders;
using FoodDelivery.Models.DominModels.Subscriptions;
using FoodDelivery.Models.DTO.PaymentDTO;
using FoodDelivery.Models.DTO.SubscriptionDTO;
using FoodDelivery.Services.Common;
using FoodDelivery.Services.Payment;
using FoodDelivery.Services.PromoCodeService;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Linq;
using System.Net;


namespace FoodDelivery.Services.Subscriptions;

public class SubscriptionServices : ISubscriptionServices
{
    private readonly DBContext _context;
    private readonly IPromoCodeService _promo;
    private readonly IPaymentService _payment;


    public SubscriptionServices(DBContext context, IPromoCodeService promo, IPaymentService payment)
    {
        _context = context;
        _promo = promo;
        _payment = payment;
    }
    public async Task<SingleResult<Guid>> AddSubscription(CreateSubscriptionRequest request)
    {
        if (!await _context.Customers.AnyAsync(x => x.Id == request.CustomerID))
            return SingleResult<Guid>.Failure(["This customer doesn't exist"], HttpStatusCode.NotFound);
        if (await _context.Subscriptions.AnyAsync(x => x.SubscriptionStatus == SubscriptionStatus.Pending && x.CustomerID == request.CustomerID))
            return SingleResult<Guid>.Failure(["you already have a pending subscription"], HttpStatusCode.Conflict);

        var subscription = new Subscription();

        if (request.PromoCodeID != null)
        {
            subscription.PromoCodeID = request.PromoCodeID;
            subscription.CustomerID = request.CustomerID;

            var result = await _promo.AddPromoCodeToSubscription(subscription);

            if (result.IsSuccess) subscription = result.Data;

            else return SingleResult<Guid>.Failure(result.Errors, result.HttpStatusCode);
        }


        subscription.ID = Guid.NewGuid();
        subscription.CustomerID = request.CustomerID;
        subscription.SubscriptionStatus = SubscriptionStatus.Pending;

        /*await _payment.SubscriptionPaymentProcess(new PaySubscriptionDTO() 
        { 
            TotalAmountInPennies = 500 ,
            CustomerID = subscription.CustomerID
        });*/

        await _context.AddAsync(subscription);
        await _context.SaveChangesAsync();
        return SingleResult<Guid>.Success(subscription.ID);

    }

    public async Task<bool> EditSubscriptionStatus(UpdateSubscriptionStatusRequest request)
    {
        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(x => x.ID == request.ID);
        if (subscription == null) return false;

        subscription.SubscriptionStatus = request.SubscriptionStatus;
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<SingleResult<bool>> AddSubscriptionDayData(CreateSubscriptionDayDataRequest request)
    {
        if (await _context.SubscriptionsDaysData.AnyAsync(
            x => x.SubscriptionID == request.SubscriptionID &&
            x.DeliveryDate == request.DeliveryDate &&
            request.CreateSubscriptionMealOptionRequests.Select(x => x.MealOptionID).Contains(x.MealOptionID)))
            return SingleResult<bool>.Failure(["A one or more identical meals is already scheduled for delivery at the same time"], HttpStatusCode.Conflict);

        if (!await _context.Subscriptions.AnyAsync(x => x.ID == request.SubscriptionID))
            return SingleResult<bool>.Failure(["This subscription doesn't exist"], HttpStatusCode.NotFound);

        if (!await _context.Subscriptions.AnyAsync(x => x.ID == request.SubscriptionID && x.SubscriptionStatus == SubscriptionStatus.Pending))
            return SingleResult<bool>.Failure(["This subscription is already paid for, please add a new subscription"], HttpStatusCode.Conflict);

        var requestMealOptionIDs = request.CreateSubscriptionMealOptionRequests.Select(x => x.MealOptionID);

        if (!requestMealOptionIDs.All(y => _context.MealOptions.Any(z => z.ID == y)))
            return SingleResult<bool>.Failure(["One or more of the meal options do not exist"], HttpStatusCode.NotFound);

        if (!await _context.MealOptions.Where(x => requestMealOptionIDs.Contains(x.ID)).AllAsync(x => x.IsAvailable))
            return SingleResult<bool>.Failure(["One or more of the meal options is not available"], HttpStatusCode.Conflict);

        var deliveryTime = TimeOnly.FromTimeSpan(request.DeliveryDate.TimeOfDay);

        if (!await _context.MealOptions
        .Where(option => requestMealOptionIDs.Contains(option.ID))
        .Select(option => option.Meal)
        .Select(meal => meal.Chief)
        .AllAsync(chief => chief.OpeningTime <= deliveryTime && deliveryTime <= chief.ClosingTime))
            return SingleResult<bool>.Failure(["One or more of the meals is not in the chief shift"], HttpStatusCode.Conflict);


        var subscription = await _context.Subscriptions.FirstAsync(x => x.ID == request.SubscriptionID);
        

        var SubscriptionDaysData = new List<SubscriptionDayData>();
        if (subscription.PromoCodeID != null)
        {
            var result = await _promo.AddPromoCodeToSubscription(subscription);

            if (result.IsSuccess) subscription = result.Data;

            else return SingleResult<bool>.Failure(result.Errors, result.HttpStatusCode);

            var queryFromDatabase = _context.MealOptions
                .Where(x => requestMealOptionIDs.Contains(x.ID) ||
                            _context.SubscriptionsDaysData
                                .Where(y => y.SubscriptionID == request.SubscriptionID)
                                .Select(z => z.MealOptionID)
                                .Contains(x.ID))
                .Include(x => x.SubscriptionsDaysData)
                .Select(x => new { x.ID, x.Price, SubscriptionsDaysQuantity = x.SubscriptionsDaysData.Sum(z => z.Quantity) });

            var queryFromClient = request.CreateSubscriptionMealOptionRequests
                .Where(h => requestMealOptionIDs.Contains(h.MealOptionID))
                .Select(h => new { h.MealOptionID, h.Quantity });

            var resultFromDatabase = await queryFromDatabase.ToListAsync();
            var resultFromClient = queryFromClient.ToList();

            var joinedResult = resultFromDatabase
                .Select(x => new { x.ID, x.Price, Quantity = x.SubscriptionsDaysQuantity + resultFromClient.Where(y => y.MealOptionID == x.ID).Sum(y => y.Quantity) })
                .ToList();

            float TotalAmount = joinedResult.Sum(kv => kv.Price * kv.Quantity);

            subscription.TotalAmount = Math.Max(TotalAmount - subscription.MaxDiscount, TotalAmount * (1 - subscription.DiscountPercentage));
            subscription.DiscountPercentage = (float)((TotalAmount - subscription.TotalAmount) / TotalAmount);

            SubscriptionDaysData = await _context.SubscriptionsDaysData.AsTracking().Where(x => x.SubscriptionID == request.SubscriptionID).ToListAsync();

            foreach (var SubscriptionDayData in SubscriptionDaysData)
            {
                SubscriptionDayData.Price = Math.Round(joinedResult.Where(x => x.ID == SubscriptionDayData.MealOptionID).Select(x => x.Price).First() * SubscriptionDayData.Quantity * (1 - (double)subscription.DiscountPercentage),2);
            }

            foreach (var CreateSubscriptionMealOptionRequest in request.CreateSubscriptionMealOptionRequests)
            {
                SubscriptionDaysData.Add(new SubscriptionDayData()
                {
                    SubscriptionID = request.SubscriptionID,
                    MealOptionID = CreateSubscriptionMealOptionRequest.MealOptionID,
                    DeliveryDate = request.DeliveryDate,
                    Quantity = CreateSubscriptionMealOptionRequest.Quantity,
                    Price = Math.Round(joinedResult.Where(x => x.ID == CreateSubscriptionMealOptionRequest.MealOptionID).Select(x => x.Price).First() * CreateSubscriptionMealOptionRequest.Quantity * (1 - (double)subscription.DiscountPercentage),2),
                    OrderStatus = OrderStatus.PendingConfirmation,
                });
                await _context.AddAsync(SubscriptionDaysData.Last());
            }
            await _context.SaveChangesAsync();

            return SingleResult<bool>.Success(true);
        }
        else
        {
            var mealOptionPrices = await _context.MealOptions.Where(x => requestMealOptionIDs.Contains(x.ID)).ToDictionaryAsync(x => x.ID, x => x.Price);

            foreach (var CreateSubscriptionMealOptionRequest in request.CreateSubscriptionMealOptionRequests)
            {
                mealOptionPrices.TryGetValue(request.SubscriptionID, out float price);
                SubscriptionDaysData.Add(new SubscriptionDayData()
                {
                    SubscriptionID = request.SubscriptionID,
                    MealOptionID = CreateSubscriptionMealOptionRequest.MealOptionID,
                    DeliveryDate = request.DeliveryDate,
                    Quantity = CreateSubscriptionMealOptionRequest.Quantity,
                    Price = Math.Round(price * CreateSubscriptionMealOptionRequest.Quantity,2),
                    OrderStatus = OrderStatus.PendingConfirmation,
                });

            }
            await _context.SubscriptionsDaysData.AddRangeAsync(SubscriptionDaysData);
            await _context.SaveChangesAsync();
            return SingleResult<bool>.Success(true);
        }
    }

    public async Task<SingleResult<GetSubscriptionRequest>> GetSubscription(Guid subscriptionID)
    {

        if (await _context.Subscriptions.AnyAsync(x => x.ID == subscriptionID))
            return SingleResult<GetSubscriptionRequest>.Failure(["This subscription doesn't exist"], HttpStatusCode.NotFound);
        var Subscription = await _context.Subscriptions.Where(x => x.ID == subscriptionID)
            .Select(x => new GetSubscriptionRequest()
            {
                SubscriptionID = x.ID,
                From = x.From,
                To = x.To,
                TotalAmount = x.TotalAmount,
                SubscriptionStatus = x.SubscriptionStatus,
                getSubscriptionDayDataRequests = x.SubscriptionDayData.Select(x => new GetSubscriptionDayDataRequest()
                {
                    MealOptionID = x.MealOptionID,
                    DeliveryDate = x.DeliveryDate,
                    MealName = x.MealOption.Meal.Name,
                    Quantity = x.Quantity,
                    OrderStatus = x.OrderStatus,
                    Price = x.Price,
                }).ToArray()
            })
            .FirstAsync();
        return SingleResult<GetSubscriptionRequest>.Success(Subscription);
    }

    public async Task<SingleResult<bool>> EditSubscriptionMealOptionQuantity(UpdateSubscriptionMealOptionQuantityRequest request)
    {
        var subscriptionDayData = await _context.SubscriptionsDaysData.AsTracking().FirstOrDefaultAsync(x => x.SubscriptionID == request.SubscriptionID && x.DeliveryDate == request.DeliveryDate && x.MealOptionID == request.MealOptionID);

        if (subscriptionDayData == null)
            return SingleResult<bool>.Failure(["This subscription does not exist"], HttpStatusCode.NotFound);


        if (await _context.Subscriptions.AnyAsync(x => x.ID == request.SubscriptionID && x.SubscriptionStatus == SubscriptionStatus.Pending))
        {
            if (subscriptionDayData.Quantity == request.Quantity)
                return SingleResult<bool>.Success(true);

            subscriptionDayData.Quantity = request.Quantity;
            await _context.SaveChangesAsync();
            return SingleResult<bool>.Success(true);
        }

        return SingleResult<bool>.Failure(["You cannot update this subscription because it has been payed for"], HttpStatusCode.Conflict);

    }

    public async Task<SingleResult<bool>> EditSubscriptionDayData(UpdateSubscriptionDayDataRequest request)
    {

        if (!await _context.MealOptions.AnyAsync(x => x.ID == request.NewMealOptionID))
            return SingleResult<bool>.Failure(["This meal option does not exist"], HttpStatusCode.NotFound);

        if (!await _context.MealOptions.AnyAsync(x => x.ID == request.NewMealOptionID && x.IsAvailable == true))
            return SingleResult<bool>.Failure(["This meal option is not avaliable"], HttpStatusCode.NotFound);

        if (await _context.SubscriptionsDaysData.AnyAsync(x => x.SubscriptionID == request.SubscriptionID && x.DeliveryDate == request.NewDeliveryDate && x.MealOptionID == request.MealOptionID))
            return SingleResult<bool>.Success(true);

        DeleteSubscriptionDayDataRequest deleteSubscriptionDayData = new()
        {
            SubscriptionID = request.SubscriptionID,
            DeliveryDate = request.DeliveryDate,
            MealOptionID = request.MealOptionID
        };

        var result = await DeleteSubscriptionDayData(deleteSubscriptionDayData);
        if (result.IsSuccess)
        {
            SubscriptionDayData subscriptionDayData = new()
            {
                SubscriptionID = request.SubscriptionID,
                MealOptionID = request.NewMealOptionID,
                DeliveryDate = request.NewDeliveryDate,
                Quantity = request.Quantity,
                OrderStatus = OrderStatus.PendingConfirmation
            };

            await _context.AddAsync(subscriptionDayData);
            await _context.SaveChangesAsync();
            return SingleResult<bool>.Success(true);
        }
        return SingleResult<bool>.Failure(result.Errors, result.HttpStatusCode);

    }

    public async Task<SingleResult<bool>> DeleteSubscriptionDayData(DeleteSubscriptionDayDataRequest request)
    {

        var SubscriptionDayData = await _context.SubscriptionsDaysData.FirstOrDefaultAsync(x => x.SubscriptionID == request.SubscriptionID && x.DeliveryDate.Date == request.DeliveryDate && x.MealOptionID == request.MealOptionID);
        if (SubscriptionDayData == null)
            return SingleResult<bool>.Failure(["This subscription data does not exist"], HttpStatusCode.NotFound);


        if (await _context.Subscriptions.AnyAsync(x => x.ID == request.SubscriptionID && x.SubscriptionStatus == SubscriptionStatus.Pending))
        {
            _context.Remove(SubscriptionDayData);
            await _context.SaveChangesAsync();
            return SingleResult<bool>.Success(true);
        }

        return SingleResult<bool>.Failure(["You cannot update this subscription because it has been payed for"], HttpStatusCode.Conflict);

    }

}
