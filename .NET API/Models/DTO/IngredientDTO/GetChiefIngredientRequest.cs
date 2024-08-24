using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.IngredientDTO;

public record GetChiefIngredientRequest
{
    public FoodIngredient Ingredient { get; init; }
    public int PricePerKilo { get; init; }
}
