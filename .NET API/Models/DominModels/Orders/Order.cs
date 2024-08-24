using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Address;
using FoodDelivery.Models.DominModels.Subscriptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models.DominModels.Orders;

public class Order
{
    public Guid ID { get; set; }
    public string CustomerID { get; set; }
    public Guid BuildingID { get; set; }
    public string FloorNo { get; set; }
    public string ApartmentNo { get; set; }
    public string PhoneNumber { get; set; }
    public string? PromoCodeID { get; set; }
    [NotMapped]
    public float DiscountPercentage { get; set; } = 0;
    [NotMapped]
    public float MaxDiscount { get; set; } = 0;
    public float TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? TimeOfDelivery { get; set; }
    public bool DeliverNow { get; set; }
    public PaymentOption PaymentOption { get; set; }
    public virtual Building Building { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual CustomerPromoCode CustomerPromoCode { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
