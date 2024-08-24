using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Meals;
using FoodDelivery.Models.Utility;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.MealDTO;

public record CreateMealRequest
{
    //public string? ChiefID { get; set; }

    public string Name { get; init; }

    public string Description { get; init; }
    //[MaxFileSize(10 * 1024 * 1024)]
    //[AllowedExtension(".jpg,.png,.jpeg")]
    //public List<IFormFile> Images { get; set; }

    [Range(0, 4, ErrorMessage = "This spice level does not exist")]
    public MealSpiceLevel MealSpiceLevel { get; init; }
    [Range(0, 2, ErrorMessage = "This meal category does not exist")]
    public MealCategory MealCategory { get; init; }
    [Range(0, 5, ErrorMessage = "This meal style does not exist")]
    public MealStyle MealStyle { get; init; }
    public List<Enums.MealTag> TagsID { get; init; }
}
