using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Meals;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.SideDishDTO;

public record CreateSideDishOptionRequest
{
    public Guid SideDishID { get; init; }
    public MealSizeOption SideDishSizeOption { get; init; }
    [Range(0, 1000)]
    public float Price { get; init; }
    [Range(0 , 100)]
    public int Quantity { get; init; }
}
