namespace FoodDelivery.Models.DTO.SubscriptionDTO;

public class CreateSubscriptionDayDataRequest
{
    public Guid SubscriptionID { get; set; }
    public DateTime DeliveryDate { get; set; }
    public ICollection<CreateSubscriptionMealOptionRequest> CreateSubscriptionMealOptionRequests { get; set; }
}
