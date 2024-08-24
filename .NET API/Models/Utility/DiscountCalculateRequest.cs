using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels;
using System.Text.Json.Serialization;

namespace FoodDelivery.Models.Utility;

public class DiscountCalculateRequest
{
    [JsonIgnore]
    public string? CustomerID { get; set; }
    public string PromoCodeID { get; set; }
    public List<MealData> MealData { get; set; }
    public List<SideDishData> SideDishData { get; set; }

}
public class MealData
{
    public Guid MealOptionID { get; set; }
    public int Quantities { get; set; }
}
public record SideDishData
{
    public Guid SideDishID { get; set; }
    public MealSizeOption SideDishSizeOption { get; set; }
    public int Quantities { get; set; }
}