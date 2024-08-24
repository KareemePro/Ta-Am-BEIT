using FoodDelivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.CartDTO;

public record UpsertCartItemRequest
{
    public Guid MealOptionID { get; init; }
    [Range(1,50)]
    public int Quantity { get; init; }
    public ICollection<SelectedSideDish>? SideDishes { get; init; }
}

public record SelectedSideDish
{
    public Guid MealSideDishID { get; init; }
    public Guid MealSideDishOptionID { get; init; }
    public MealSizeOption SideDishSizeOption { get; init; }
}
