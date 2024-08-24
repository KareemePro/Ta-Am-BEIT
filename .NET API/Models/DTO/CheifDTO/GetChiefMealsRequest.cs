using FoodDelivery.Models.DTO.MealDTO;

namespace FoodDelivery.Models.DTO.CheifDTO;

public record GetChiefMealsRequest
{
    public string ChiefID { get; init; }
    public string ChiefName { get; init; }
    public string PrfileImage { get; init; }
    public string CoverImage { get; init; }
    public bool IsOnline { get; init; }
    public string Description { get; init; }
    public int ReviewCount { get; init; }
    public float Rating { get; init; }
    public int OrdersDone { get; init; }
    public ICollection<GetMealRequest> Meals { get; init; }
}
