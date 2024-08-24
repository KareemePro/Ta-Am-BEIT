using FoodDelivery.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models.DominModels.Subscriptions;

public class Subscription
{
    public Guid ID { get; set; }
    public string CustomerID { get; set; }
    public string? PromoCodeID { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    [NotMapped]
    public float DiscountPercentage { get; set; } = 0;
    [NotMapped]
    public float MaxDiscount { get; set; } = 0;
    public float? TotalAmount { get; set; }
    public SubscriptionStatus SubscriptionStatus { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual CustomerPromoCode CustomerPromoCode { get; set; }
    public ICollection<SubscriptionDayData> SubscriptionDayData { get; set;}
}
