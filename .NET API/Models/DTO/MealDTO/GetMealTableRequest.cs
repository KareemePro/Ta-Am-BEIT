using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.MealDTO;

public record GetMealTableRequest
{
    public Guid MealID { get; init; }

    public string Title { get; init; }

    public MealCategory Category { get; init; }

    public MealSpiceLevel SpiceLevel { get; init; }

    public MealStyle MealStyle { get; init; } 

    public float Rating { get; init; }

    public bool IsAvailable { get; init; }
    public ICollection<GetMealOptionTable> getMealOptionsTable { get; init; }

}
public record GetMealOptionTable
{
    public Guid MealOptionID { get; init; }
    public MealSizeOption MealSizeOption { get; init; }
    public float Price { get; init; }
    public int Sold { get; init; }
    public bool IsAvailable { get; init; }
    public string ThumbnailImage { get; init; }
}
