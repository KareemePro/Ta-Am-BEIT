using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Meals;
using FoodDelivery.Models.DominModels.Orders;
using FoodDelivery.Models.DominModels.Subscriptions;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DominModels;

public class MealOption
{
    public Guid ID { get; set; }
    public Guid MealID { get; set; }
    [Display(Name = "Is Available")]
    public MealSizeOption MealSizeOption { get; set; }
    public bool IsAvailable { get; set; }
    public float Price { get; set; }
    public int? AvailableQuantity { get; set; }
    public int? DailyQuantity { get; set; }
    public string ThumbnailImage { get; set; }
    public string FullScreenImage { get; set; }
    public virtual Meal Meal { get; set; }
    public ICollection<MealOptionIngredient> MealOptionIngredients { get; set; } = new List<MealOptionIngredient>();
    //public ICollection<MealSideDish> MealSideDishes { get; set; }
    //[Range(0, 2, ErrorMessage = "This meal size does not exist")]
    public ICollection<MealSideDish> MealSideDishes { get; set; } = new List<MealSideDish>();
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<SubscriptionDayData> SubscriptionsDaysData { get; set; }
}
