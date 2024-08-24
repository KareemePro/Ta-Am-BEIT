using FoodDelivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.SubscriptionDTO
{
    public class UpdateSubscriptionStatusRequest
    {
        public Guid ID { get; set; }
        [Range(0 , 2)]
        public SubscriptionStatus SubscriptionStatus { get; set; }
    }
}
