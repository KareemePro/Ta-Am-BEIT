namespace FoodDelivery.Models.DTO.MealDTO;

public class UpdateMealOptionRequest
{
    public Guid MealOptionID { get; set; }
    public bool IsAvailable { get; set; }
    public float Price { get; set; }
    public int? AvailableQuantity { get; set; }
    public bool SaveQuantitySetting { get; set; }
    public string? Image { get; set; }
    public ICollection<AddMealSideDish> MealSideDishes { get; set; } = new List<AddMealSideDish>();
    public ICollection<AddIngredient> AddIngredients { get; set; } = new List<AddIngredient>();
}
