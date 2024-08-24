namespace FoodDelivery.Models.DTO.MealDTO;

public record GetChiefAnalyticsRequest
{
    public string ChiefID { get; init; }
    public string ChiefName { get; init; }
    public string Image { get; init;}
    public string PhoneNumber { get; init; }
    public float TotalRevenue { get; init; }
    public float TotalCost { get; init; }
}
