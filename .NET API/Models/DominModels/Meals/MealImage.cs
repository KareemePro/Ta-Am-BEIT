namespace FoodDelivery.Models.DominModels;

public class MealImage
{
    public Guid ID { get; set; }

    public Guid MealID { get; set; }

    public string Image { get; set; }

    public virtual Meal Meal { get; set; }
}
