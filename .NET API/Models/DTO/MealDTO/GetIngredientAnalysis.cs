using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.MealDTO;

public record GetIngredientAnalysis
{
    public FoodIngredient Ingredient { get; init; }
    public int UsedAmountInGrams { get; init; }
    public float CostPerKilo { get; init; }
}
