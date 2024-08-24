using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.SideDishDTO;

public record GetSideDishRequest
{
    public Guid SideDishID { get; init; }
    public string Name { get; init; }
    public string ThumbnailImage { get; init; }
    public string FullScreenImage { get; init; }
    public ICollection<GetSideDishOption> GetSideDishOptions { get; init; }
    public GetSideDishRequest() { }
}

public record GetSideDishOption
{ 
    public MealSizeOption SideDishSizeOption { get; init; }
    public float Price { get; init; }
    public int AvailableQuantity { get; init; }
    public GetSideDishOption() { }
}
