using FoodDelivery.Enums;

namespace FoodDelivery.Models.DTO.SubscriptionDTO;

public record GetSubscriptionRequest
{
    /*public GetSubscriptionRequest(Guid SubscriptionID,
    DateTime From,
    DateTime To,
    SubscriptionStatus SubscriptionStatus,
    double TotalAmount)
    {
        this.SubscriptionID = SubscriptionID;
        this.From = From;
        this.To = To;
        this.SubscriptionStatus = SubscriptionStatus;
        this.TotalAmount = TotalAmount;
    }*/
    public Guid SubscriptionID { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public SubscriptionStatus SubscriptionStatus { get; set;}
    public double? TotalAmount { get; init; }
    public ICollection<GetSubscriptionDayDataRequest> getSubscriptionDayDataRequests { get; init; }

    public GetSubscriptionRequest() { }

}

public record GetSubscriptionDayDataRequest
{
    public Guid MealOptionID { get; init; }
    public DateTime DeliveryDate { get; init; }
    public string MealName { get; init; }
    public int Quantity { get; init; }
    public double? Price { get; init; }
    public OrderStatus OrderStatus { get; init; }

    public GetSubscriptionDayDataRequest() { }
}
