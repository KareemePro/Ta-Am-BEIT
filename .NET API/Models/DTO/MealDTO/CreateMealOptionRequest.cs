using FoodDelivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.MealDTO;

public record CreateMealOptionRequest
{
    public Guid MealID { get; init; }
    public MealSizeOption MealSizeOption { get; init; }
    public bool IsAvailable { get; init; }
    public float Price { get; init; }
    public int? AvailableQuantity { get; init; }
    public bool SaveQuantitySetting { get; init; }
    public string Image { get; init; }
    public List<AddMealSideDish> MealSideDishes { get; init; }
    public List<AddIngredient>? AddIngredients { get; init; }
}

public record AddMealSideDish
{
    public bool IsFree { get; init; }
    public bool IsTopping { get; init; }
    public List<AddMealSideDishOption> SideDishOptions { get; init; }
}

public record AddMealSideDishOption
{
    public Guid SideDishID { get; init; }
    public MealSizeOption SideDishSizeOption { get; init; }
}

public record AddIngredient
{
    public FoodIngredient FoodIngredient { get; init; }
    public int AmountInGrams { get; init; }
}