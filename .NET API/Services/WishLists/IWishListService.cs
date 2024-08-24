using FoodDelivery.Models.DTO.WishListDTO;
using FoodDelivery.Services.Common;

namespace FoodDelivery.Services.WishLists;

public interface IWishListService
{
    Task<SingleResult<bool>> AddList(CreateWishListItemsRequest request);
    Task<bool> AddItem(string UserID, Guid MealOptionID);
}
