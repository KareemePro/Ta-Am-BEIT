using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DominModels;

public class ChiefReview
{
    public string CustomerID { get; set; }

    public string ChiefID { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
    [Range(0, 5)]
    public int? Rating { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Chief Chief { get; set; }
}
