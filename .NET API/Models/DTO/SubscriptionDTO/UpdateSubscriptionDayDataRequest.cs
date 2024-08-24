using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.SubscriptionDTO;

public class UpdateSubscriptionDayDataRequest
{
    public Guid SubscriptionID { get; set; }
    public DateTime DeliveryDate { get; set; }
    public Guid MealOptionID { get; set; }
    public DateTime NewDeliveryDate { get; set; }

    public Guid NewMealOptionID { get; set; }

    [Range(1, 50)]
    public int Quantity { get; set; }


}
