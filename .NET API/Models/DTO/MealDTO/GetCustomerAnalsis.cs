namespace FoodDelivery.Models.DTO.MealDTO;

public record GetCustomerAnalsis
{
    public string CustomerID { get; init; }
    public string CustomerImage { get; init; }
    public string CustomerName { get; init; }
    public int TotalOrders { get; init; }
    public double TotalCost { get; init; }
    public double TotalRevenue { get; init; }
}
