using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Subscriptions;

namespace FoodDelivery.Models.DominModels.Orders;

public class OrderItem
{
    public int OrderItemID { get; set; }
    public Guid OrderID { get; set; }
    public Guid MealOptionID { get; set; }
    public DateTime DeliveryDate { get; set; }
    public int Quantity { get; set; }
    public float PricePerUnit { get; set; }
    public float TotalAmount { get; set; }
    public float TotalCost { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public bool IsDelivered { get; set; }
    public virtual Order Order { get; set; }
    public virtual MealOption MealOption { get; set; }
    public ICollection<OrderItemOption> OrderItemOptions { get; set; }
}
