using Azure.Core;
using FoodDelivery.Data;
using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DominModels.Orders;
using FoodDelivery.Models.DominModels.Subscriptions;
using FoodDelivery.Models.DTO.MealDTO;
using FoodDelivery.Models.DTO.OrderDTO;
using FoodDelivery.Models.DTO.PaymentDTO;
using FoodDelivery.Models.Utility;
using FoodDelivery.Services.Common;
using FoodDelivery.Services.Payment;
using FoodDelivery.Services.PromoCodeService;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Transactions;
using X.Paymob.CashIn.Models.Payment;

namespace FoodDelivery.Services.Orders;

public class OrderService : IOrderService
{
    private readonly DBContext _context;
    private readonly IPromoCodeService _promo;
    private readonly IPaymentService _payment;


    public OrderService(DBContext context, IPromoCodeService promo, IPaymentService payment)
    {
        _context = context;
        _promo = promo;
        _payment = payment;

    }

    public async Task<SingleResult<bool>> UpdateOrderItemStatus(int OrderItemID, string ChiefID, OrderStatus OrderItemUpdate)
    {
        var OrderItem = await _context.OrderItems.AsTracking().FirstOrDefaultAsync(x => x.OrderItemID == OrderItemID && x.MealOption.Meal.ChiefID == ChiefID);

        if (OrderItem == null)
            return SingleResult<bool>.Failure(["this order item does not exist"], HttpStatusCode.NotFound);

        OrderItem.OrderStatus = OrderItemUpdate;

        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }

    public async Task<SingleResult<CashInPaymentKeyResponse>> CreateOrder(CreateOrderRequest request, Guid CustomerID)
    {
        var Customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == CustomerID.ToString());
        if (Customer == null)
            return SingleResult<CashInPaymentKeyResponse>.Failure(["This customer does not exist"], HttpStatusCode.NotFound);

        if (!await _context.Buildings.AnyAsync(x => x.ID == request.BuildingID))
            return SingleResult<CashInPaymentKeyResponse>.Failure(["This building does not exist"], HttpStatusCode.NotFound);
        //error
        //var cart.Items = await _context.cart.Items.Where(c => c.UserID == CustomerID.ToString()).AsTracking().Include(x => x.SelectedSideDishes).ToListAsync();
        var cart = await _context.Cart.Include(x => x.Items)
            .ThenInclude(y => y.ItemOptions)
            .ThenInclude(n => n.MealSideDish)
            .Include(x => x.Items)
            .ThenInclude(y => y.ItemOptions)
            .ThenInclude(n => n.SideDishOption)
            .Where(c => c.UserID == CustomerID.ToString())
            .AsTracking()
            .FirstOrDefaultAsync();
        //var cart.Items = await _context.CartItems
        //    .Include(x => x.ItemOptions)
        //        .ThenInclude(x => x.MealSideDish)
        //    .Include(x => x.ItemOptions)
        //        .ThenInclude(x => x.SideDishOption)
        //    .Where(c => c.UserID == CustomerID.ToString())
        //    .AsTracking()
        //    .ToListAsync();
        if (cart.Items == null) return SingleResult<CashInPaymentKeyResponse>.Failure(["Your cart is empty"], HttpStatusCode.NotFound);

        var mealOptionIDs = cart.Items.Select(m => m.MealOptionID);

        var sideDishOptionIDs = cart.Items.Select(x => x.ItemOptions.Select(y => new { y.MealSideDishOptionID, y.SideDishSizeOption }));

        if (!await _context.MealOptions.Where(x => mealOptionIDs.Contains(x.ID)).AllAsync(x => x.IsAvailable))
            return SingleResult<CashInPaymentKeyResponse>.Failure(["One or more of the meal options is not available"], HttpStatusCode.Conflict);

        Order order = new();

        if (request.PromoCodeID != null && request.PromoCodeID != "")
        {
            order.PromoCodeID = request.PromoCodeID;
            order.CustomerID = CustomerID.ToString();

            var result = await _promo.AddPromoCodeToOrder(order);

            if (result.IsSuccess) order = result.Data;

            else return SingleResult<CashInPaymentKeyResponse>.Failure(result.Errors, result.HttpStatusCode);
        }


        order.ID = Guid.NewGuid();
        order.CustomerID = CustomerID.ToString();
        order.ApartmentNo = request.ApartmentNo;
        order.BuildingID = request.BuildingID;
        order.FloorNo = request.FloorNo;
        order.PaymentOption = request.PaymentOption;
        order.OrderDate = DateTime.UtcNow;
        order.PhoneNumber = request.PhoneNumber;
        order.TimeOfDelivery = request.TimeOfDelivery != null ? new DateTime(2024, 6, 13, request.TimeOfDelivery.Value.Hour, request.TimeOfDelivery.Value.Minute, 0): null;
        order.DeliverNow = cart.DeliverNow;


        var orderItemsRequest = new List<OrderItem>();

        //var mealOptionPrices = await _context.MealOptions
        //    .Include(x => x.MealOptionIngredients)
        //    .ThenInclude(x => x.FoodIngredient)
        //    .Where(x => mealOptionIDs.Contains(x.ID))
        //    .ToDictionaryAsync(x => x.ID, x => x);

        var mealOptionPrices = await _context.MealOptions
            .Include(x => x.MealOptionIngredients)
            .ThenInclude(x => x.ChiefIngredient)
            .Where(x => mealOptionIDs.Contains(x.ID))
            .ToDictionaryAsync(x => x.ID, x => x);


        //error
        //var selectedSideDishes = cart.Items.SelectMany(x => x.SelectedSideDishes).ToList();
        //var selectedSideDishes = cart.Items.SelectMany(x => x.ItemOptions).ToList();
        //var sideDishIDs = selectedSideDishes.Select(y => y.MealSideDishOptionID).ToList();
        //var sideDishSizeOptions = selectedSideDishes.Select(y => y.SideDishSizeOption).ToList();

        //var sideDishesPrices = await _context.MealOptions
        //    .Where(x => mealOptionIDs.Contains(x.ID))
        //    .SelectMany(x => x.MealSideDishes)
        //    .Where(x => !x.IsFree)
        //    .SelectMany(x => x.MealSideDishOptions)
        //    .Where(x => sideDishIDs.Contains(x.SideDishID) && sideDishSizeOptions.Contains(x.SideDishSizeOption))
        //    .Select(x => new { x.SideDishOption.Price, x.SideDishID, x.SideDishSizeOption })
        //    .ToDictionaryAsync();

        //var sideDishesPrices = await _context.MealOptions
        //    .Where(x => mealOptionIDs.Contains(x.ID))
        //    .SelectMany(x => x.MealSideDishes)
        //    .Where(x => !x.IsFree)
        //    .SelectMany(x => x.MealSideDishOptions)
        //    .Where(x => sideDishIDs.Contains(x.SideDishID) && sideDishSizeOptions.Contains(x.SideDishSizeOption))
        //    .ToDictionaryAsync(x => new { x.SideDishID , x.SideDishSizeOption}, x => x.SideDishOption.Price);

        //var sideDishesPrices = await _context.MealOptions
        //    .Where(x => mealOptionIDs.Contains(x.ID))
        //    .SelectMany(x => x.MealSideDishes)
        //    .Where(x => !x.IsFree)
        //    .SelectMany(x => x.MealSideDishOptions)
        //    .Where(x => sideDishIDs.Contains(x.SideDishID) && sideDishSizeOptions.Contains(x.SideDishSizeOption))
        //    .Select(z => new { z.MealSideDish.MealOptionID, z.SideDishID, z.SideDishSizeOption, z.SideDishOption.Price, z.MealSideDish.IsFree, z.SideDishOption.SideDish.Name })
        //    .ToListAsync();

    //    var sideDishesPrices = await _context.MealSideDishes
    //        .Where(x => selectedSideDishes.Select(y => y.MealSideDishID).Contains(x.ID))
    //        .Where(x => x.IsFree == false)
    //        .SelectMany(x => x.MealSideDishOptions)
    //        .Where(x => sideDishIDs.Contains(x.SideDishID) && sideDishSizeOptions.Contains(x.SideDishSizeOption))
    //        .Select(z => new { z.MealSideDish.MealOptionID, z.MealSideDishID ,z.SideDishID, z.SideDishSizeOption, z.SideDishOption.Price, z.MealSideDish.IsFree, z.SideDishOption.SideDish.Name })
    //        .ToListAsync();

    //    var sideDishesPrices1 = await _context.MealSideDishes
    //.Where(x => selectedSideDishes.Select(y => y.MealSideDishID).Contains(x.ID))
    //.Select(x => x.MealSideDishOptions)
    //.ToListAsync();

    //    var f = cart.Items.Select(x => x.ItemOptions.Where(y => y.MealSideDish.IsFree == false).Select(u =>new { u.SideDishOption.Price, x.Quantity }));



        //var mealSideDishes = await _context.MealOptions.Where(x => mealOptionIDs.Contains(x.ID)).Select(x => x.MealSideDishes).ToListAsync();

        //foreach (var mealSideDish in mealSideDishes)
        //{
        //    foreach(var sideDishes in mealSideDish)
        //    {
        //        if (sideDishes.IsFree)
        //            continue;
        //        foreach (var dish in sideDishes.MealSideDishOptions)
        //        {
        //            dish.SideDishOption.Price
        //        }
        //    }
        //}

        if (request.PromoCodeID != null)
        {
            float TotalAmount = mealOptionPrices.Sum(kv =>
            {
                var cartItems = cart.Items.Where(item => item.MealOptionID == kv.Key);
                if (cartItems.Any())
                {
                    return kv.Value.Price * cartItems.Sum(item => item.Quantity);
                }
                return 0;
            }) + cart.Items.Select(x => x.ItemOptions.Where(y => y.MealSideDish.IsFree == false).Sum(a => a.SideDishOption.Price * x.Quantity)).Sum() + 20;

            order.TotalAmount = Math.Max(TotalAmount - order.MaxDiscount, TotalAmount * (1 - order.DiscountPercentage));
            order.DiscountPercentage = (TotalAmount - order.TotalAmount) / TotalAmount;
            foreach (var item in cart.Items)
            {
                mealOptionPrices.TryGetValue(item.MealOptionID, out MealOption mealOption);

                orderItemsRequest.Add(new OrderItem
                {
                    OrderID = order.ID,
                    PricePerUnit = mealOption.Price * (1 - (float)order.DiscountPercentage),
                    TotalAmount = ((mealOption.Price + item.ItemOptions.Where(y => y.MealSideDish.IsFree == false).Sum(x => x.SideDishOption.Price)) * item.Quantity) * (1 - (float)order.DiscountPercentage),
                    TotalCost = mealOption.MealOptionIngredients.Sum(x => x.AmountInGrams * ((float)x.ChiefIngredient.CostPerKilo / 1000)) * item.Quantity,
                    IsDelivered = false,
                    Quantity = item.Quantity,
                    MealOptionID = item.MealOptionID,
                    OrderStatus = OrderStatus.PendingConfirmation,
                    OrderItemOptions = item.ItemOptions.Select(option => new OrderItemOption()
                    {
                        SideDishID = option.MealSideDishOptionID,
                        SideDishSizeOption = option.SideDishSizeOption,
                        IsFree = option.MealSideDish.IsFree,
                        PricePerUnit = option.SideDishOption.Price,
                        Quantity = 1
                    }).ToArray()

                });
            }
        }
        else
        {
            foreach (var item in cart.Items)
            {
                mealOptionPrices.TryGetValue(item.MealOptionID, out MealOption mealOption);
                var itemTotalAmount = (mealOption.Price + item.ItemOptions.Where(y => y.MealSideDish.IsFree == false).Sum(x => x.SideDishOption.Price)) * item.Quantity;
                orderItemsRequest.Add(new OrderItem
                {
                    OrderID = order.ID,
                    PricePerUnit = mealOption.Price,
                    TotalAmount = itemTotalAmount,
                    IsDelivered = false,
                    Quantity = item.Quantity,
                    TotalCost = mealOption.MealOptionIngredients.Sum(x => x.AmountInGrams * (x.ChiefIngredient.CostPerKilo / 1000)),
                    MealOptionID = item.MealOptionID,
                    OrderStatus = OrderStatus.PendingConfirmation,
                    OrderItemOptions = item.ItemOptions.Select(option => new OrderItemOption()
                    {
                        SideDishID = option.MealSideDishOptionID,
                        SideDishSizeOption = option.SideDishSizeOption,
                        IsFree = option.MealSideDish.IsFree,
                        PricePerUnit = option.SideDishOption.Price,
                        Quantity = 1
                    }).ToArray()

                });
                order.TotalAmount += itemTotalAmount;
            }
        }

        CashInPaymentKeyResponse paymentToken = new CashInPaymentKeyResponse();

        if (order.PaymentOption != PaymentOption.CashOnDelivery)
        {
            PaymentDTO paymentDTO = new()
            {
                OrderID = order.ID,
                TotalAmountInPennies = ((int)order.TotalAmount ) * 100,
                FirstName = Customer.FirstName,
                LastName = Customer.LastName,
                Email = Customer.Email,
                BuildingID = order.BuildingID,
                PhoneNumber = order.PhoneNumber,
                FloorNo = request.FloorNo,
                ApartmentNo = request.ApartmentNo

                
            };
            paymentToken = await _payment.OrderPaymentProcess(paymentDTO);
        }

        await _context.Orders.AddAsync(order);
        await _context.OrderItems.AddRangeAsync(orderItemsRequest);

        foreach (var item in orderItemsRequest)
        {
            await _context.AddRangeAsync(item.OrderItemOptions);
        }

        if (order.PaymentOption == PaymentOption.CashOnDelivery)
        {
            await _context.SaveChangesAsync();
        }
        //error
        //_context.cart.Items.RemoveRange(cart.Items);
        //await _context.SaveChangesAsync();

        //foreach (var orderItem in order.OrderItems)
        //{
        //    await _notification.SendNewOrderNotification("dfghgf", orderItem);
        //}
        return SingleResult<CashInPaymentKeyResponse>.Success(paymentToken);
    }

    public async Task<ListResult<GetOrderItem>> GetChiefOrders(string ChiefID)
    {
        var OrderItems = await _context.OrderItems.Where(x => x.MealOption.Meal.ChiefID == ChiefID).Select(orderItem => new GetOrderItem()
        {
            OrderItemID = orderItem.OrderItemID,
            MealOptionID = orderItem.MealOptionID,
            MealID = orderItem.MealOption.MealID,
            MealName = orderItem.MealOption.Meal.Name,
            MealOptionImage = orderItem.MealOption.FullScreenImage,
            DeliveryDate = orderItem.Order.TimeOfDelivery,
            Quantity = orderItem.Quantity,
            PricePerUnit = orderItem.PricePerUnit,
            ExpectedTimeOfDelivery = orderItem.Order.DeliverNow ?
                new TimeOnly(orderItem.Order.OrderDate.TimeOfDay.Hours, orderItem.Order.OrderDate.TimeOfDay.Minutes) :
                new TimeOnly(DateTime.Now.AddMinutes(30).Hour, DateTime.Now.AddMinutes(30).Minute),
            TotalAmount = orderItem.TotalAmount,
            OrderStatus = orderItem.OrderStatus,
            GetOrderItemOprions = orderItem.OrderItemOptions.Select(orderItemOption => new GetOrderItemOprion()
            {
                SideDishID = orderItemOption.SideDishID,
                SideDishSizeOption = orderItemOption.SideDishSizeOption,
                SideDishName = orderItemOption.SideDishOption.SideDish.Name,
                IsFree = orderItemOption.IsFree,
                PricePerUnit = orderItemOption.PricePerUnit
            }).ToArray()
        }).ToArrayAsync();

        if (OrderItems == null)
            return ListResult<GetOrderItem>.Failure(["you have no order to display"], HttpStatusCode.NotFound);

        return ListResult<GetOrderItem>.Success(OrderItems);
    }

    public async Task<ListResult<GetOrderRequest>> GetOrderDetails(string CustomerID)
    {
        var Order = await _context.Orders.Where(x => x.CustomerID == CustomerID).Select(order => new GetOrderRequest()
        {
            OrderID = order.ID,
            District = order.Building.Street.District.Name,
            Street = order.Building.Street.Name,
            Building = order.Building.Name,
            FloorNo = order.FloorNo,
            ApartmentNo = order.ApartmentNo,
            PhoneNumber = order.PhoneNumber,
            PromoCodeID = order.PromoCodeID,
            DiscountPercentage = order.DiscountPercentage,
            TotalAmount = order.TotalAmount,
            OrderDate = order.OrderDate,
            PaymentOption = order.PaymentOption,
            GetOrderItems = order.OrderItems.Select(orderItem => new GetOrderItem() 
            {
                OrderItemID = orderItem.OrderItemID,
                MealID = orderItem.MealOption.MealID,
                MealOptionID = orderItem.MealOptionID,
                MealName = orderItem.MealOption.Meal.Name,
                ChiefName = orderItem.MealOption.Meal.Chief.FirstName + " " + orderItem.MealOption.Meal.Chief.LastName,
                MealOptionImage = orderItem.MealOption.FullScreenImage,
                DeliveryDate = orderItem.DeliveryDate,
                Quantity = orderItem.Quantity,
                PricePerUnit = orderItem.PricePerUnit,
                TotalAmount = orderItem.TotalAmount,
                OrderStatus = orderItem.OrderStatus,
                GetOrderItemOprions = orderItem.OrderItemOptions.Select(orderItemOption => new GetOrderItemOprion()
                {
                    SideDishID = orderItemOption.SideDishID,
                    SideDishSizeOption = orderItemOption.SideDishSizeOption,
                    SideDishName = orderItemOption.SideDishOption.SideDish.Name,
                    IsFree = orderItemOption.IsFree,
                    PricePerUnit = orderItemOption.PricePerUnit
                }).ToArray()
            }).ToArray()
        }).ToArrayAsync();

        if (Order == null)
            return ListResult<GetOrderRequest>.Failure(["This order does not exist"], HttpStatusCode.NotFound);

        return ListResult<GetOrderRequest>.Success(Order);
    }

    public async Task<ListResult<GetOrderSummaryRequest>> GetOrdersSummary(string CustomerID)
    {
        var Orders = await _context.Orders.Where(x => x.CustomerID == CustomerID).Select(order => new GetOrderSummaryRequest()
        {
            OrderID = order.ID,
            OrderDate = order.OrderDate,
            TimeOfDelivery = order.TimeOfDelivery,
            DeliverNow = order.DeliverNow,
            TotalAmount = order.TotalAmount,
            District = order.Building.Street.District.Name,
            Street = order.Building.Street.Name,
            Building = order.Building.Name,
            FloorNo = order.FloorNo,
            ApartmentNo = order.ApartmentNo,
            PhoneNumber = order.PhoneNumber,
            PaymentOption = order.PaymentOption
        }).ToArrayAsync();

        if (Orders == null)
            return ListResult<GetOrderSummaryRequest>.Failure(["you do not have any orders"], HttpStatusCode.NotFound);

        return ListResult<GetOrderSummaryRequest>.Success(Orders);
    }
}

