namespace FoodDelivery.Models.DTO.PaymentDTO;

public class PaySubscriptionDTO
{
    public Guid SubscriptionID { get; set; }    
    public int TotalAmountInPennies { get; set; }
    public string CustomerID { get; set; }
}
