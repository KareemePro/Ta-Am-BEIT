using FoodDelivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DominModels;

public class ChiefIngredient
{
    public string ChiefID { get; set; }
    public FoodIngredient FoodIngredient { get; set; }
    public int CostPerKilo { get; set; }
    public bool Visible { get; set; }
    public virtual Chief Chief { get; set; }
    public ICollection<MealOptionIngredient> MealOptionIngredients { get; set; } = new List<MealOptionIngredient>();
}
