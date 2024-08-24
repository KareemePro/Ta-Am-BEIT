using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Meals;

namespace FoodDelivery.Models.DominModels;

public class MealSideDishOption
{
    public Guid MealSideDishID { get; set; }
    public Guid SideDishID { get; set; }
    public MealSizeOption SideDishSizeOption { get; set; }
    public virtual MealSideDish MealSideDish { get; set; }
    public virtual SideDishOption SideDishOption { get; set; }
}
