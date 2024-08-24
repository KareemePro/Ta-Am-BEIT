namespace FoodDelivery.Models.DTO.WishListDTO;

public class CreateWishListItemsRequest
{
    public string UserID { get; set; }
    public List<Guid> MealOptionIDs { get; set;}
}
