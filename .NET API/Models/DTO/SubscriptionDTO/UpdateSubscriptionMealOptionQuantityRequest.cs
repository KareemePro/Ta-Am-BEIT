using Microsoft.VisualBasic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.SubscriptionDTO
{
    public class UpdateSubscriptionMealOptionQuantityRequest
    {
        public Guid SubscriptionID { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Guid MealOptionID { get; set; }

        [Range(1,50)]
        public int Quantity { get; set; }

    }
}
