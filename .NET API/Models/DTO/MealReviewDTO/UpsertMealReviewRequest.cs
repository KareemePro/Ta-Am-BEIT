using FoodDelivery.Models.DominModels;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.MealReviewDTO;

public class UpsertMealReviewRequest
{
    public Guid MealID { get; set; }

    public string Text { get; set; }

    [Range(0, 5)]
    public int Rating { get; set; }

    public IFormFile? ReviewImage { get; set; }
}
