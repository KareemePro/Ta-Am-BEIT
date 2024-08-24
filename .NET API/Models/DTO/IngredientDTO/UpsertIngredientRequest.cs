using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.IngredientDTO;

public record UpsertIngredientRequest
{
    public FoodIngredient Ingredient { get; init; }
    public int PricePerKilo { get; init; }
    public bool Delete { get; init; }
}
