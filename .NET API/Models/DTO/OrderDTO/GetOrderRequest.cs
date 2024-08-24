using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.OrderDTO;

public record GetOrderRequest
{
    public Guid OrderID { get; init; }
    public string District { get; init; }
    public string Street { get; init; }
    public string Building { get; init; }
    public string FloorNo { get; init; }
    public string ApartmentNo { get; init; }
    public string PhoneNumber { get; init; }
    public string? PromoCodeID { get; init; }
    public float? DiscountPercentage { get; init; }
    public float TotalAmount { get; init; }
    public DateTime OrderDate { get; init; }
    public PaymentOption PaymentOption { get; init; }
    public ICollection<GetOrderItem> GetOrderItems { get; init; }
}

public record GetOrderItem
{
    public int OrderItemID { get; init; }
    public Guid MealOptionID { get; init; }
    public Guid MealID { get; init; }
    public string MealName { get; init; }
    public string ChiefName { get; init; }
    public TimeOnly ExpectedTimeOfDelivery { get; init; }
    public string MealOptionImage { get; init; }
    public DateTime? DeliveryDate { get; init; }
    public int Quantity { get; init; }
    public float PricePerUnit { get; init; }
    public float TotalAmount { get; init; }
    public OrderStatus OrderStatus { get; init; }
    public ICollection<GetOrderItemOprion> GetOrderItemOprions { get; init; }
}

public record GetOrderItemOprion
{
    public Guid SideDishID { get; init; }
    public MealSizeOption SideDishSizeOption { get; init; }
    public string SideDishName { get; init; }
    public bool IsFree { get; init; }
    public float PricePerUnit { get; init; }
}
