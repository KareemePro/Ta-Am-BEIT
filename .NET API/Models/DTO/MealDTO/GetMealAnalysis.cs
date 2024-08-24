using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.MealDTO;

public record GetMealAnalysis
{
    public Guid MealID { get; init; }
    public Guid MealOptionID { get; init; }
    public MealSizeOption MealSizeOption { get; init; }
    public string Name { get; init;}
    public string Image { get; init; }
    public int SoldAmount { get; init; }
    public float TotalCost { get; init; }
    public float TotalRevenue { get; init; }
}
