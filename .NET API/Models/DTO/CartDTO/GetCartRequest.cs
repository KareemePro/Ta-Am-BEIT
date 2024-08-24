using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.CartDTO;

public record GetCartRequest
{
    public DateTime? TimeOfDelivery { get; set; }
    public bool DeliverNow { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public ICollection<GetCartItemRequest>? CartItems { get; set; }
}

public record GetCartItemRequest
{
    public int CartItemID { get; set; }
    public Guid MealOptionID { get; set; }
    public MealSizeOption MealSizeOption { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public string Image { get; set; }
    public int Quantity { get; set; }
    public float TotalPrice { get; set; }
    public int AvailableQuantity { get; set; }
    public ICollection<GetCartItemOptionRequest>? CartItemOptions { get; set; }
}

public record GetCartItemOptionRequest
{
    public Guid MealSideDishID { get; set; }
    public Guid MealSideDishOptionID { get; set; }
    public string Name { get; set; }
    public bool IsFree { get; set; }
    public bool IsTopping { get; set; }
    public float? Price { get; set; }
    public MealSizeOption SideDishSizeOption { get; set; }
}
