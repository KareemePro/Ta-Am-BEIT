namespace FoodDelivery.Models.DominModels.Meals;

public class MealSideDish
{
    public Guid ID { get; set; }
    public Guid MealOptionID { get; set; }
    public bool IsFree { get; set; }
    public bool IsTopping { get; set; }
    public virtual MealOption MealOption { get; set; }
    public ICollection<MealSideDishOption> MealSideDishOptions { get; set; } = new List<MealSideDishOption>();
}
