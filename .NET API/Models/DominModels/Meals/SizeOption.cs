namespace FoodDelivery.Models.DominModels;

public class SizeOption
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public ICollection<MealOption> MealOptions { get; set; }
}
