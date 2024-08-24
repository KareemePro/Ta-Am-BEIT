using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.MealDTO;

public record GetCartMealRequest
{
    public Guid MealOptionID { get; init; }
    public Guid MealID { get; init; }
    public string Title { get; init; }
    public float Rating { get; init; }
    public MealSpiceLevel MealSpiceLevel { get; init; }
    public MealCategory MealCategory { get; init; }
    public MealStyle MealStyle { get; init; }
    public float Price { get; init; }
    public int AvailableQuantity { get; init; }
    public string Image { get; init; }
}

