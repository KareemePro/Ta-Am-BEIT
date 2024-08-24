using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Meals;
using System.Text.Json.Serialization;

namespace FoodDelivery.Models.DominModels;

public class SelectedSideDish
{
    [JsonIgnore]
    public string UserID { get; set; }
    [JsonIgnore]
    public Guid MealOptionID { get; set; }
    public Guid SideDishID { get; set; }
    public MealSizeOption SideDishSizeOption { get; set; }
    [JsonIgnore]
    public virtual Cart Cart { get; set; }
    [JsonIgnore]
    public virtual SideDishOption SideDishOption { get; set; }
}
