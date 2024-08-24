using FoodDelivery.Enums;

namespace FoodDelivery.Models.DominModels;

public class MealOptionIngredient
{
    public Guid MealOptionID { get; set; }
    public FoodIngredient FoodIngredient { get; set; }
    public string ChiefID { get; set; }
    public int AmountInGrams { get; set; }
    public virtual MealOption MealOption { get; set; }
    public virtual ChiefIngredient ChiefIngredient { get; set; }
}
