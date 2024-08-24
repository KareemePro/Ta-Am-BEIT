using FoodDelivery.Models.DominModels.Address;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DominModels;

public class Chief : User
{
    //[Required(ErrorMessage = "Please enter your middle name")]
    //[MinLength(2, ErrorMessage = "A name must be at least 2 letters")]
    //[MaxLength(15, ErrorMessage = "A name must be not more than 15 letters")]
    //[Display(Name = "Middle Name")]
    //public string MiddleName { get; set; }

    public string? GovernmentID { get; set; }
    [Display(Name = "Is Available")]
    public bool IsAvailable { get; set; }

    public string? Description { get; set; }

    public string? ChiefFullScreenImage { get; set; }

    public string? CoverImage { get; set; }

    public string? HealthCertImage { get; set; }

    public string? FloorNumber { get; set; }

    public string? ApartmentNumber { get; set; }

    public TimeOnly OpeningTime { get; set; } = new TimeOnly(9, 0, 0);

    public TimeOnly ClosingTime { get; set; } = new TimeOnly(20, 0, 0);

    public DateTime CreatedDate { get; set; }

    public virtual ChiefReview ChiefReview { get; set; }

    public ICollection<Meal> Meals { get; set; }
}
