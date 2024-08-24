using FoodDelivery.Enums;

namespace FoodDelivery.Models.DominModels.Subscriptions;

public class SubscriptionDayData
{
    public Guid SubscriptionID { get; set; }
    public Guid MealOptionID { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public int Quantity { get; set; }
    public double? Price { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public virtual Subscription Subscription { get; set; }
    public virtual MealOption MealOption { get; set; }
}