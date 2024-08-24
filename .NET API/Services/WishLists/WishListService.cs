using FoodDelivery.Data;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.WishListDTO;
using FoodDelivery.Services.Common;
using Mailjet.Client.Resources;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.WishLists;

public class WishListService : IWishListService
{
    private readonly DBContext _context;

    public WishListService(DBContext context)
    {
        _context = context;
    }

    public async Task<SingleResult<bool>> AddList(CreateWishListItemsRequest request)
    {
        if (!await _context.Users.AnyAsync(x => x.Id == request.UserID))
            return SingleResult<bool>.Failure(["please try to login in again"]);

        var ValidMealOptionCount = await _context.MealOptions.CountAsync(x => request.MealOptionIDs.Contains(x.ID));

        if (ValidMealOptionCount == request.MealOptionIDs.Count)
        {
            foreach (var MealOptionID in request.MealOptionIDs)
            {
                await AddItem(request.UserID, MealOptionID);
            }
            await _context.SaveChangesAsync();
            return SingleResult<bool>.Success(true);
        }
        return SingleResult<bool>.Failure(["one or more meal option do not exist"]);
    }

    public async Task<bool> AddItem(string UserID, Guid MealOptionID)
    {
        if (await _context.WishLists.AnyAsync(x => x.UserID == UserID && x.MealOptionID == MealOptionID))
            return true;
        await _context.WishLists.AddAsync(new WishList { MealOptionID = MealOptionID, UserID = UserID });
        return true;
    }
}
