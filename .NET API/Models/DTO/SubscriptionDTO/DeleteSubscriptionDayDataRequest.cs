namespace FoodDelivery.Models.DTO.SubscriptionDTO
{
    public class DeleteSubscriptionDayDataRequest
    {
        public Guid SubscriptionID { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Guid MealOptionID { get; set; }
    }
}
