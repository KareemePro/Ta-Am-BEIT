using FoodDelivery.Models.DominModels.Orders;
using FoodDelivery.Models.DominModels.Subscriptions;

namespace FoodDelivery.Models.DominModels;

public class CustomerPromoCode
{
    public string PromoCodeID { get; set; }
    public string CustomerID { get; set; }
    public bool IsUsed { get; set; }
    public bool IsUsedByOrder { get; set; }
    public DateTime? UsedDate { get; set; }
    public virtual PromoCode PromoCode { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual Order Order { get; set; }
    public virtual Subscription Subscription { get; set; }
}
