using FoodDelivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.MealDTO;

public class UpdateMealRequest
{
    public Guid MealID { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    [Range(0, 4, ErrorMessage = "This spice level does not exist")]
    public MealSpiceLevel? MealSpiceLevel { get; set; }
    [Range(0, 2, ErrorMessage = "This meal category does not exist")]
    public MealCategory? MealCategory { get; set; }
    [Range(0, 5, ErrorMessage = "This meal style does not exist")]
    public MealStyle? MealStyle { get; set; }

    public List<Enums.MealTag>? TagsID { get; set; }

    public IFormFile? Image { get; set; }
}
