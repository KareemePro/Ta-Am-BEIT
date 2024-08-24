using FoodDelivery.Data;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.CartDTO;
using FoodDelivery.Services.Common;
using FoodDelivery.Services.WishLists;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FoodDelivery.Services.CartService;

public class CartService : ICartService
{
    private readonly DBContext _context;

    private readonly IWishListService _wishListService;

    public CartService(DBContext context, IWishListService wishListService)
    {
        _context = context;

        _wishListService = wishListService;
    }


    public async Task<SingleResult<bool>> UpsertCartItem(UpsertCartItemRequest request, string UserID)
    {
        if (!await _context.Users.AnyAsync(x => x.Id == UserID))
            return SingleResult<bool>.Failure(["This user does not exist"], HttpStatusCode.NotFound);

        var MealOption = await _context.MealOptions.FirstOrDefaultAsync(x => x.ID == request.MealOptionID);

        //if (!await _context.MealOptions.AnyAsync(x => x.ID == request.MealOptionID))
        if (MealOption == null)
            return SingleResult<bool>.Failure(["This meal option does not exist"], HttpStatusCode.NotFound);

        //if (!await _context.MealOptions.AnyAsync(x => x.ID == request.MealOptionID && x.IsAvailable == true))
        if (!MealOption.IsAvailable)
            return SingleResult<bool>.Failure(["This meal option is not available"], HttpStatusCode.Conflict);

        if (!(MealOption.AvailableQuantity >= request.Quantity))
            return SingleResult<bool>.Failure(["This Quantity is not available"], HttpStatusCode.Conflict);

        //if (request.TimeOfDelivery != null && !await _context.Chiefs.AnyAsync(x => x.Id == MealOption.Meal.ChiefID && MealOption.Meal.Chief.OpeningTime <= request.TimeOfDelivery && MealOption.Meal.Chief.ClosingTime >= request.TimeOfDelivery))
        //    return CartResult<Cart>.Failure(["This Time Of Delivery is outside the chief schedule"], HttpStatusCode.Conflict);

        //    var sortedRequestSideDishes = request.SideDishes
        //.OrderBy(sd => sd.MealSideDishID)
        //.ThenBy(sd => sd.MealSideDishOptionID)
        //.ToList();

        //    // Check if a CartItem with the same MealOptionID and SideDishes exists
        //    var exists = await _context.CartItems
        //        .Include(ci => ci.ItemOptions)  // Include ItemOptions in the query
        //        .AnyAsync(ci => ci.Cart.UserID == UserID
        //            && ci.MealOptionID == request.MealOptionID
        //            && ci.ItemOptions
        //                .OrderBy(io => io.MealSideDishID)
        //                .ThenBy(io => io.MealSideDishOptionID)
        //                .All(x => x.MealSideDishID == sortedRequestSideDishes.)
        //                .SequenceEqual(sortedRequestSideDishes, new SideDishComparer()));


        //    return exists;
        var cartItem = new CartItem();
        if (request.SideDishes != null)
        {
            cartItem = await _context.CartItems
            .Include(ci => ci.MealOption)
            .Include(ci => ci.ItemOptions)
            .ThenInclude(io => io.MealSideDish)
            .ThenInclude(msd => msd.MealSideDishOptions)
            .FirstOrDefaultAsync(ci =>
                ci.Cart.UserID == UserID &&
                ci.MealOptionID == request.MealOptionID &&
                ci.ItemOptions.All(io =>
                    io.MealSideDishID == request.SideDishes.FirstOrDefault(sd => sd.MealSideDishID == io.MealSideDishID).MealSideDishOptionID));

        }

        //var cartItem = await _context.Cart.AsTracking().Where(x => x.UserID == UserID && x.Items.Where(x => x.MealOptionID == request.MealOptionID
        // && x.ItemOptions.All(x => x.MealSideDishID.)))

        //var cart = await _context.Cart.AsTracking().FirstOrDefaultAsync(x => x.UserID == UserID && x.MealOptionID == request.MealOptionID);
        if (cartItem != null)
        {
            if (cartItem.Quantity == request.Quantity)
            {
                var getCartItem = new GetCartItemRequest()
                {
                    CartItemID = cartItem.ID,
                    MealOptionID = cartItem.MealOptionID,
                    Quantity = cartItem.Quantity,
                    CartItemOptions = cartItem.ItemOptions.Select(x => new GetCartItemOptionRequest()
                    {
                        MealSideDishID = x.MealSideDishID,
                        MealSideDishOptionID = x.MealSideDishOptionID,
                        SideDishSizeOption = x.SideDishSizeOption
                    }).ToArray()
                };
                return SingleResult<bool>.Success(true);
            }

            cartItem.Quantity = request.Quantity;
            await _context.SaveChangesAsync();
            return SingleResult<bool>.Success(true);
        }

        if (!await _context.Cart.AnyAsync(x => x.UserID == UserID))
        {
            Cart cart = new()
            {
                UserID = UserID,
                DeliverNow = true
            };
            await _context.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        CartItem newCartItem = new()
        {
            UserID = UserID,
            MealOptionID = request.MealOptionID,
            Quantity = request.Quantity,
        };

        await _context.AddAsync(newCartItem);
        await _context.SaveChangesAsync();

        if (request.SideDishes != null)
        {
            newCartItem.ItemOptions = request.SideDishes.Select(x => new CartItemOption()
            {
                CartItemID = newCartItem.ID,
                MealSideDishID = x.MealSideDishID,
                MealSideDishOptionID = x.MealSideDishOptionID,
                SideDishSizeOption = x.SideDishSizeOption
            }).ToArray();
        }
        await _context.AddRangeAsync(newCartItem.ItemOptions);
        await _context.SaveChangesAsync();
        return SingleResult<bool>.Success(true);


    }



    public async Task<bool> DeleteCartItem(DeleteCartItemRequest request, string UserID)
    {
        var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.UserID == UserID && x.ID == request.CartItemID);

        if (cartItem == null) return false;

        _context.Remove(cartItem);
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<CartResult<GetCartRequest>> RefreshCart(Guid UserID, List<UpsertCartItemRequest>? request, TimeOnly? TimeOfDelivery)
    {
        if (!await _context.Users.AnyAsync(x => x.Id == UserID.ToString()))
            return CartResult<GetCartRequest>.Failure(["please try to login again"]);

        var UserCart = await _context.Cart.Where(x => x.UserID == UserID.ToString()).Include(x => x.Items).ThenInclude(x => x.ItemOptions).FirstOrDefaultAsync();

        if (UserCart == null)
        {
            UserCart = new Cart()
            {
                UserID = UserID.ToString(),
                TimeOfDelivery = TimeOfDelivery != null ? DateTime.Today.AddHours(TimeOfDelivery.Value.Hour).AddMinutes(TimeOfDelivery.Value.Minute) : null,
                DeliverNow = TimeOfDelivery == null
            };
            await _context.AddAsync(UserCart);
            await _context.SaveChangesAsync();
        }

        UserCart.TimeOfDelivery = TimeOfDelivery != null ? DateTime.Today.AddHours(TimeOfDelivery.Value.Hour).AddMinutes(TimeOfDelivery.Value.Minute) : null;
        UserCart.DeliverNow = TimeOfDelivery == null;

        List<CartItem>? newCartItems = [];

        if (request != null)
        {
            newCartItems = request.Select(item => new CartItem
            {
                MealOptionID = item.MealOptionID,
                Quantity = item.Quantity,
                UserID = UserID.ToString(),
                ItemOptions = item.SideDishes == null ? new List<CartItemOption>() :
                item.SideDishes.Select(option => new CartItemOption
                {
                    MealSideDishID = option.MealSideDishID,
                    MealSideDishOptionID = option.MealSideDishOptionID,
                    SideDishSizeOption = option.SideDishSizeOption,
                }).ToList()
            }).ToList();
        }
        if (UserCart.Items != null || newCartItems != null)
            return await ValidateCart(newCartItems, UserCart);


        return CartResult<GetCartRequest>.Failure(["you have no items added to your cart"]);
    }

    //private async Task<CartResult<Cart>> ValidateCart(List<Cart> newCartItems, List<Cart> MergedCartItems, List<Cart> CartItems)
    //{

    //    var changes = new List<string>();

    //    var cartItemsDict = CartItems.ToDictionary(c => c.MealOptionID);


    //    foreach (var cartItem in newCartItems)
    //    {
    //        var MealOption = await _context.MealOptions.Include(x => x.Meal).ThenInclude(x => x.Chief).FirstOrDefaultAsync(x => x.ID == cartItem.MealOptionID);

    //        if (MealOption == null)
    //        {
    //            changes.Add($"{cartItem.MealOptionID} does not exist");
    //            continue;
    //        }


    //        if (!MealOption.IsAvailable)
    //        {
    //            await _wishListService.AddItem(cartItem.UserID, MealOption.ID);
    //            changes.Add($"{MealOption.Meal.Name} is not available & been added to your wish list");
    //        }

    //        if (!(MealOption.AvailableQuantity >= cartItem.Quantity))
    //        {
    //            cartItem.Quantity = (int)MealOption.AvailableQuantity;
    //            changes.Add($"Quantity for {MealOption.Meal.Name} is not available, it is been changed to {MealOption.AvailableQuantity}");
    //        }

    //        if (!await _context.Chiefs.AnyAsync(x => x.Id == MealOption.Meal.ChiefID && MealOption.Meal.Chief.OpeningTime <= cartItem.TimeOfDelivery && MealOption.Meal.Chief.ClosingTime >= cartItem.TimeOfDelivery))
    //        {
    //            var openingTimeDifference = (MealOption.Meal.Chief.OpeningTime - cartItem.TimeOfDelivery);
    //            var closingTimeDifference = (MealOption.Meal.Chief.ClosingTime - cartItem.TimeOfDelivery);

    //            cartItem.TimeOfDelivery = openingTimeDifference < closingTimeDifference ? MealOption.Meal.Chief.OpeningTime : MealOption.Meal.Chief.ClosingTime;
    //            changes.Add($"your time of delivery for{MealOption.Meal.Name} has been changed to {cartItem.TimeOfDelivery}");
    //        }
    //    }

    //    _context.UpdateRange(CartItems);
    //    await _context.AddRangeAsync(newCartItems);
    //    await _context.SaveChangesAsync();

    //    if (changes.Count != 0)
    //    {
    //        return CartResult<Cart>.Modified(changes, MergedCartItems);
    //    }

    //    return CartResult<Cart>.Correct(MergedCartItems);
    //}

    private async Task<CartResult<GetCartRequest>> ValidateCart(List<CartItem>? newCartItems, Cart UserCart)
    {
        var changes = new List<string>();

        //var cartItemsDict = UserCart.Items?
        //    .GroupBy(c => c.MealOptionID)
        //    .ToDictionary(
        //        g => g.Key,
        //        g => g.ToDictionary(
        //            item => new
        //            {
        //                MealSideDishID = GetCollectionHashCode(item.ItemOptions.Select(io => io.MealSideDishID)),
        //                MealSideDishOptionID = GetCollectionHashCode(item.ItemOptions.Select(io => io.MealSideDishOptionID)),
        //                SideDishSizeOption = GetCollectionHashCode(item.ItemOptions.Select(io => io.SideDishSizeOption))
        //            }.GetHashCode(),
        //            item => item
        //        )
        //    )
        //    ?? new Dictionary<Guid, Dictionary<int, CartItem>>();

        //if (newCartItems != null)
        //{
        //    // Process newCartItems
        //    for (int i = newCartItems.Count - 1; i >= 0; i--)
        //    {
        //        var cartItem = newCartItems[i];

        //        if (cartItemsDict.TryGetValue(cartItem.MealOptionID, out var existingItemsDict))
        //        {
        //            var cartItemHashCode = new
        //            {
        //                MealSideDishID = GetCollectionHashCode(cartItem.ItemOptions.Select(io => io.MealSideDishID)),
        //                MealSideDishOptionID = GetCollectionHashCode(cartItem.ItemOptions.Select(io => io.MealSideDishOptionID)),
        //                SideDishSizeOption = GetCollectionHashCode(cartItem.ItemOptions.Select(io => io.SideDishSizeOption))
        //            }.GetHashCode();

        //            if (existingItemsDict.TryGetValue(cartItemHashCode, out var existingCartItem))
        //            {


        //                existingCartItem.Quantity = cartItem.Quantity;

        //                newCartItems.RemoveAt(i);
        //                existingItemsDict.Remove(cartItemHashCode);
        //                continue;
        //            }
        //        }


        //        changes = await ValidateCartItem(cartItem, changes);

        //    }
        //}
        // Integration into your existing code
        var cartItemsDict = UserCart.Items
            .GroupBy(c => c.MealOptionID)
            .ToDictionary(
                g => g.Key,
                g => g.ToDictionary(
                    item => new
                    {
                        MealSideDishIDs = item.ItemOptions.Select(io => io.MealSideDishID).ToList(),
                        MealSideDishOptionIDs = item.ItemOptions.Select(io => io.MealSideDishOptionID).ToList(),
                        SideDishSizeOptions = item.ItemOptions.Select(io => io.SideDishSizeOption).ToList()
                    },
                    item => item
                )
            );

        if ( cartItemsDict.Count == 0 && (newCartItems == null || newCartItems.Count == 0) ) 
        {
            return CartResult<GetCartRequest>.Correct(new GetCartRequest());
        }

        if (newCartItems != null)
        {
            // Process newCartItems
            for (int i = newCartItems.Count - 1; i >= 0; i--)
            {
                var cartItem = newCartItems[i];

                if (cartItemsDict.TryGetValue(cartItem.MealOptionID, out var existingItemsDict))
                {
                    var cartItemProperties = new
                    {
                        MealSideDishIDs = cartItem.ItemOptions.Select(io => io.MealSideDishID).ToList(),
                        MealSideDishOptionIDs = cartItem.ItemOptions.Select(io => io.MealSideDishOptionID).ToList(),
                        SideDishSizeOptions = cartItem.ItemOptions.Select(io => io.SideDishSizeOption).ToList()
                    };

                    // Check if any existing item matches the current one based on properties
                    var existingCartItem = existingItemsDict.Values.FirstOrDefault(item => AreItemsExactlySame(item, cartItem));

                    if (existingCartItem != null)
                    {
                        existingCartItem.Quantity = cartItem.Quantity;

                        newCartItems.RemoveAt(i);
                        //existingItemsDict.Remove(cartItem);
                        continue;
                    }
                }

                changes = await ValidateCartItem(cartItem, changes);
            }
        }

        if (UserCart.Items != null && UserCart.Items.Count > 0)
        {
            foreach (var cartItem in UserCart.Items)
            {
                changes = await ValidateCartItem(cartItem, changes);
            }
        }

        // ... rest of your code ...
        if (UserCart.Items != null && UserCart.Items.Count > 0)
        {
            _context.UpdateRange(UserCart.Items);
        }
        if (newCartItems != null && newCartItems.Count > 0)
        {
            await _context.AddRangeAsync(newCartItems);
        }
        await _context.SaveChangesAsync();

        var combinedCartItems = new List<CartItem>(UserCart.Items ?? new List<CartItem>());

        var latestOpeningTime = new TimeOnly();
        var earliestClosingTime = new TimeOnly();

        var chiefsWithMealOptions = await _context.Chiefs
            .Where(chief => chief.Meals.Any(meal => meal.MealOptions.Any(option =>
                combinedCartItems.Select(x => x.MealOptionID).Contains(option.ID))))
            .Select(x => new { x.OpeningTime, x.ClosingTime })
            .ToListAsync();
        if(chiefsWithMealOptions.Count > 0)
        {

            latestOpeningTime = chiefsWithMealOptions.Max(x => x.OpeningTime);
            earliestClosingTime = chiefsWithMealOptions.Min(x => x.ClosingTime);
        }


        if (newCartItems != null)
        {
            //combinedCartItems.AddRange(newCartItems);

            if (!UserCart.DeliverNow)
            {

                var timeOfDelivery = TimeOnly.FromTimeSpan(UserCart.TimeOfDelivery == null ? DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 0, 0)) : UserCart.TimeOfDelivery.Value.TimeOfDay);

                if (earliestClosingTime <= timeOfDelivery && latestOpeningTime >= timeOfDelivery)
                {
                    var openingTimeDifference = earliestClosingTime - timeOfDelivery;
                    var closingTimeDifference = latestOpeningTime - timeOfDelivery;

                    timeOfDelivery = openingTimeDifference < closingTimeDifference ? earliestClosingTime : latestOpeningTime;

                    UserCart.TimeOfDelivery = DateTime.Today.AddHours(timeOfDelivery.Hour).AddMinutes(timeOfDelivery.Minute);
                }

            }
        }

        UserCart.Items = combinedCartItems;

        var cartItems = UserCart.Items.ToList();

        var getCart = new GetCartRequest()
        {
            TimeOfDelivery = UserCart.TimeOfDelivery,
            DeliverNow = UserCart.DeliverNow,
            StartTime = earliestClosingTime,
            EndTime = latestOpeningTime,
            CartItems = new List<GetCartItemRequest>()
        };

        foreach (var cartItem in cartItems)
        {
            var MealOption = await _context.MealOptions
                .Include(x => x.Meal)
                    .Where(x => x.ID == cartItem.MealOptionID)
                    .Select(x => new { x.Price, x.FullScreenImage, x.Meal.Name, x.MealSizeOption, x.AvailableQuantity })
                    .FirstOrDefaultAsync();

            var getCartItem = new GetCartItemRequest()
            {
                CartItemID = cartItem.ID,
                MealOptionID = cartItem.MealOptionID,
                MealSizeOption = MealOption.MealSizeOption,
                Name = MealOption.Name,
                Quantity = cartItem.Quantity,
                AvailableQuantity = MealOption.AvailableQuantity ?? 3,
                Price = MealOption.Price,
                TotalPrice = MealOption.Price,
                Image = MealOption.FullScreenImage,
                CartItemOptions = new List<GetCartItemOptionRequest>()
            };

            foreach (var itemOption in cartItem.ItemOptions)
            {
                var IsFreeAndIsTopping = await _context.MealSideDishes.Where(x => x.ID == itemOption.MealSideDishID).Select(x => new { x.IsFree , x.IsTopping}).FirstOrDefaultAsync();
                var sideDishPriceAndName = await _context.SideDishOptions
                    .Include(x => x.SideDish)
                    .Where(y => y.SideDishID == itemOption.MealSideDishOptionID && y.SideDishSizeOption == itemOption.SideDishSizeOption)
                    .Select(x => new { x.Price,x.SideDish.Name})
                    .FirstOrDefaultAsync();

                var cartItemOption = new GetCartItemOptionRequest()
                {
                    MealSideDishID = itemOption.MealSideDishID,
                    MealSideDishOptionID = itemOption.MealSideDishOptionID,
                    SideDishSizeOption = itemOption.SideDishSizeOption,
                    IsFree = IsFreeAndIsTopping.IsFree,
                    IsTopping = IsFreeAndIsTopping.IsTopping,
                    Name = sideDishPriceAndName.Name,
                    Price = IsFreeAndIsTopping.IsFree ? 0 : sideDishPriceAndName.Price
                };
                getCartItem.TotalPrice += (float)cartItemOption.Price;
                getCartItem.CartItemOptions.Add(cartItemOption);
            }

            getCart.CartItems.Add(getCartItem);
        }



        if (changes.Count != 0)
        {
            return CartResult<GetCartRequest>.Modified(changes, getCart);
        }

        return CartResult<GetCartRequest>.Correct(getCart);
    }


    private async Task<List<string>> ValidateCartItem(CartItem CartItems, List<string> changes)
    {
        var MealOption = await _context.MealOptions.Include(x => x.Meal).ThenInclude(x => x.Chief).FirstOrDefaultAsync(x => x.ID == CartItems.MealOptionID);

        if (MealOption == null)
        {
            changes.Add($"{CartItems.MealOptionID} does not exist");
            return changes;
        }


        if (!MealOption.IsAvailable)
        {
            await _wishListService.AddItem(CartItems.UserID, MealOption.ID);
            changes.Add($"{MealOption.Meal.Name} is not available & been added to your wish list");
        }

        if (!(MealOption.AvailableQuantity >= CartItems.Quantity))
        {
            CartItems.Quantity = (int)MealOption.AvailableQuantity;
            changes.Add($"Quantity for {MealOption.Meal.Name} is not available, it is been changed to {MealOption.AvailableQuantity}");
        }

        //if (!await _context.Chiefs.AnyAsync(x => x.Id == MealOption.Meal.ChiefID && (CartItems.TimeOfDelivery == new TimeOnly(0, 0, 0) || MealOption.Meal.Chief.OpeningTime <= CartItems.TimeOfDelivery && MealOption.Meal.Chief.ClosingTime >= CartItems.TimeOfDelivery)))
        //{
        //    var openingTimeDifference = (MealOption.Meal.Chief.OpeningTime - CartItems.TimeOfDelivery);
        //    var closingTimeDifference = (MealOption.Meal.Chief.ClosingTime - CartItems.TimeOfDelivery);

        //    CartItems.TimeOfDelivery = openingTimeDifference < closingTimeDifference ? MealOption.Meal.Chief.OpeningTime : MealOption.Meal.Chief.ClosingTime;
        //    changes.Add($"your time of delivery for {MealOption.Meal.Name} {MealOption.MealSizeOption} has been changed to {CartItems.TimeOfDelivery}");
        //}

        return changes;
    }
    public int GetCollectionHashCode<T>(IEnumerable<T> collection)
    {
        int hash = 0;
        foreach (var item in collection)
        {
            hash = hash ^ item.GetHashCode();
        }
        return hash;
    }

    // Function to compare two lists
    bool AreListsEqual<T>(List<T> list1, List<T> list2)
    {
        if (list1 == null && list2 == null)
            return true;

        if (list1 == null || list2 == null)
            return false;

        if (list1.Count != list2.Count)
            return false;

        for (int i = 0; i < list1.Count; i++)
        {
            if (!list1[i].Equals(list2[i]))
                return false;
        }

        return true;
    }

    // Function to compare two items
    bool AreItemsEqual(CartItem item1, CartItem item2)
    {
        return AreListsEqual(item1.ItemOptions.Select(io => io.MealSideDishID).ToList(),
                             item2.ItemOptions.Select(io => io.MealSideDishID).ToList()) &&
               AreListsEqual(item1.ItemOptions.Select(io => io.MealSideDishOptionID).ToList(),
                             item2.ItemOptions.Select(io => io.MealSideDishOptionID).ToList()) &&
               AreListsEqual(item1.ItemOptions.Select(io => io.SideDishSizeOption).ToList(),
                             item2.ItemOptions.Select(io => io.SideDishSizeOption).ToList());
    }

    // Comparing items
    bool AreItemsExactlySame(CartItem item1, CartItem item2)
    {
        return item1 != null && item2 != null &&
               item1.ItemOptions.Select(io => io.MealSideDishID).SequenceEqual(item2.ItemOptions.Select(io => io.MealSideDishID)) &&
               item1.ItemOptions.Select(io => io.MealSideDishOptionID).SequenceEqual(item2.ItemOptions.Select(io => io.MealSideDishOptionID)) &&
               item1.ItemOptions.Select(io => io.SideDishSizeOption).SequenceEqual(item2.ItemOptions.Select(io => io.SideDishSizeOption));
    }

    public async Task<SingleResult<bool>> ChangeCartItemQTY(int amount,int CartItemID, string UserID)
    {
        var CartItem = await _context.CartItems.AsTracking().Where(x => x.ID == CartItemID && x.UserID == UserID).FirstOrDefaultAsync();
        if (CartItem == null)
            return SingleResult<bool>.Failure(["Refresh your cart"], HttpStatusCode.NotFound);

        CartItem.Quantity += amount;

        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }
}