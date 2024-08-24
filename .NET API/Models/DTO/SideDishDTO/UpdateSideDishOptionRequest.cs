using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.SideDishDTO;

public class UpdateSideDishOptionRequest
{
    public Guid SideDishID { get; init; }
    public MealSizeOption SideDishSizeOption { get; init; }
    public float? Price { get; init; }
    public int? Quantity { get; init; }
}
