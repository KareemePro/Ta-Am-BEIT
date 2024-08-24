
namespace FoodDelivery.Models.DominModels.Meals;

public class MealTag
{
    public Guid MealID { get; set; }
    public Enums.MealTag Tag { get; set; }
    public virtual Meal Meal { get; set; }
}
