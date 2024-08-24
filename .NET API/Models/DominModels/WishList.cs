namespace FoodDelivery.Models.DominModels;

public class WishList
{
    public string UserID { get; set; }
    public Guid MealOptionID { get; set; }
    public virtual User User { get; set; }
    public virtual MealOption MealOption { get; set; }
}
