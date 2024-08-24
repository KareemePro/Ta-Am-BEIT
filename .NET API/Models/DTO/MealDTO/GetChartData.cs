namespace FoodDelivery.Models.DTO.MealDTO;

public record GetChartData
{
    public int OrderCount { get; init; }
    public DateTime Date { get; init; }
}
