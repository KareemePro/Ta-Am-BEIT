using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.OrderDTO;

public record GetOrderSummaryRequest
{
    public Guid OrderID { get; init; }
    public DateTime OrderDate { get; init; }
    public DateTime? TimeOfDelivery { get; init; }
    public bool DeliverNow { get; init; }
    public float TotalAmount { get; set; }
    public string District { get; init; }
    public string Street { get; init; }
    public string Building { get; init; }
    public string FloorNo { get; init; }
    public string ApartmentNo { get; init; }
    public string PhoneNumber { get; init; }
    public PaymentOption PaymentOption { get; set; }
}
