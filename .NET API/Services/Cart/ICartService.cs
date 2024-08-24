using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.CartDTO;
using FoodDelivery.Services.Common;

namespace FoodDelivery.Services.CartService
{
    public interface ICartService
    {
        Task<SingleResult<bool>> UpsertCartItem(UpsertCartItemRequest request,string UserID);

        Task<SingleResult<bool>> ChangeCartItemQTY(int amount,int CartItemID, string UserID);

        Task<CartResult<GetCartRequest>> RefreshCart(Guid UserID, List<UpsertCartItemRequest>? request, TimeOnly? TimeOfDelivery);

        Task<bool> DeleteCartItem(DeleteCartItemRequest request, string UserID);

    }
}
