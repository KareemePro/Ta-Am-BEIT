using FoodDelivery.Enums;

namespace FoodDelivery.Models.DominModels.Meals;

public class SideDishOption
{
    public Guid SideDishID { get; set; }
    public MealSizeOption SideDishSizeOption { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
    public virtual SideDish SideDish { get; set; }
}
