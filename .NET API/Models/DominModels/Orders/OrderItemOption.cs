using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Meals;

namespace FoodDelivery.Models.DominModels.Orders;

public class OrderItemOption
{
    public int ID { get; set; }
    public int OrderItemID { get; set; }
    public Guid SideDishID { get; set; }
    public MealSizeOption SideDishSizeOption { get; set; }
    public bool IsFree { get; set; }
    public float PricePerUnit { get; set; }
    public int Quantity { get; set; }
    public virtual OrderItem OrderItem { get; set; }
    public virtual SideDishOption SideDishOption { get; set; }
}
