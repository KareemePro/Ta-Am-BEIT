using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models.DominModels;

public class MealReview
{
    public Guid MealID { get; set; }

    public string CustomerID { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }
    [Range(0 , 5)]
    public float Rating { get; set; }

    public string? ThumbnailImage { get; set; }
    public string? FullScreenImage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Meal Meal { get; set; }
}
