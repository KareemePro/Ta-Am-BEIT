namespace FoodDelivery.Models.DTO.SubscriptionDTO;

public class CreateSubscriptionRequest
{
    public string CustomerID { get; set; } = string.Empty;

    public string? PromoCodeID { get; set; }
}
