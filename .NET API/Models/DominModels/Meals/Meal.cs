using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Meals;
using FoodDelivery.Models.DominModels.Orders;
using FoodDelivery.Models.DominModels.Subscriptions;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DominModels;

public class Meal
{
    public Guid ID { get; set; }

    public string ChiefID { get; set; } = string.Empty;
    [MinLength(3, ErrorMessage = "A name must be at least 3 letters")]
    [MaxLength(20, ErrorMessage = "A name must be not more than 20 letters")]
    public string Name { get; set; } = string.Empty;
    [MinLength(10, ErrorMessage = "A description must be at least 10 letters")]
    [MaxLength(200, ErrorMessage = "A description must be not more than 200 letters")]
    public string Description { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }

    public bool IsSideDish { get; set; }

    [Range(0, 4, ErrorMessage ="This spice level does not exist")]
    public MealSpiceLevel MealSpiceLevel { get; set; }
    [Range(0, 2 , ErrorMessage = "This meal category does not exist")]
    public MealCategory MealCategory { get; set; }
    [Range(0, 5, ErrorMessage = "This meal style does not exist")]
    public MealStyle MealStyle { get; set; }
    [Range(0,5)]
    public float Rating { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime UpdatedDate { get; set; } = DateTime.Now;

    public virtual Chief Chief { get; set; }

    public ICollection<Models.DominModels.Meals.MealTag> MealTags { get; set; } = new List<Meals.MealTag>();

    public ICollection<MealReview> MealReviews { get; set; }

    public ICollection<MealOption> MealOptions { get; set; } = new List<MealOption>();

    //public ICollection<MealTag> MealTags { get; set; } = new List<MealTag>();
    public int CalculateSimilarity(Meal other)
    {
        int score = 0;

        if (this.MealCategory == other.MealCategory)
            score += 1;
        if (this.MealSpiceLevel == other.MealSpiceLevel)
            score += 1;
        if (this.MealStyle == other.MealStyle)
            score += 1;
        if (this.ChiefID == other.ChiefID)
            score += 1;

        return score;
    }
}