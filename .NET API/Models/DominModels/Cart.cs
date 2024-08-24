using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Meals;

namespace FoodDelivery.Models.DominModels;

//public class Cart
//{
//    [JsonIgnore]
//    public string UserID { get; set; }
//    public Guid MealOptionID { get; set; }
//    public int Quantity { get; set; }
//    public TimeOnly TimeOfDelivery { get; set; }
//    public ICollection<SelectedSideDish> SelectedSideDishes { get; set; }
//    [JsonIgnore]
//    public virtual User User { get; set; }
//    [JsonIgnore]
//    public virtual MealOption MealOption { get; set; }
//}
//public class Cart
//{
//    public string UserID { get; set; }
//    public DateTime TimeOfDelivery { get; set; }
//    public virtual User User { get; set; }
//    public ICollection<CartItem> Items { get; set;}
//}

//public class CartItem
//{
//    public string UserID { get; set; }
//    public Guid MealOptionID { get; set; }
//    public virtual MealOption MealOption { get; set; }
//    public virtual Cart Cart { get; set; }
//    public ICollection<CartItemOption> ItemOptions { get; set; }
//}

//public class CartItemOption
//{
//    public string UserID { get; set; }
//    public Guid MealOptionID { get; set; }
//    public Guid MealSideDishID { get; set; }
//    public Guid MealSideDishOptionID { get; set; }
//    public int Quantity { get; set; }
//    public virtual CartItem CartItem { get; set; }
//    public virtual MealSideDish MealSideDish { get; set; }
//}
public class Cart
{
    public string UserID { get; set; }
    public DateTime? TimeOfDelivery { get; set; }
    public bool DeliverNow { get; set; }
    public virtual User User { get; set; }
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}

public class CartItem
{
    public int ID { get; set; }
    public string UserID { get; set; }
    public Guid MealOptionID { get; set; }
    public int Quantity { get; set; }
    public virtual MealOption MealOption { get; set; }
    public virtual Cart Cart { get; set; }
    public ICollection<CartItemOption> ItemOptions { get; set; } = new List<CartItemOption>();
}

public class CartItemOption
{
    public int ID { get; set; }
    public int CartItemID { get; set; }
    public Guid MealSideDishID { get; set; }
    public Guid MealSideDishOptionID { get; set; }
    public MealSizeOption SideDishSizeOption { get; set; }
    public virtual CartItem CartItem { get; set; }
    public virtual MealSideDish MealSideDish { get; set; }
    public virtual SideDishOption SideDishOption { get; set; }
}

