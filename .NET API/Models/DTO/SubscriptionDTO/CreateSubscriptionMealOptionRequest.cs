using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.SubscriptionDTO;

public class CreateSubscriptionMealOptionRequest
{
    public Guid MealOptionID { get; set; }

    [Range(1,50)]
    public int Quantity { get; set; }

}
