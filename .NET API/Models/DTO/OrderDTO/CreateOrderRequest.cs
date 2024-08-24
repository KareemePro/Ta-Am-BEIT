using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.OrderDTO;

public class CreateOrderRequest
{
    public Guid BuildingID { get; set; }
    public string FloorNo { get; set; }
    public string ApartmentNo { get; set; }
    public string PromoCodeID { get; set; }
    public string PhoneNumber { get; set; }
    public PaymentOption PaymentOption { get; set; }
    public TimeOnly? TimeOfDelivery { get; init; }
}
