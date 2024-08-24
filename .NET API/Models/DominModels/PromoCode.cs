using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DominModels;

public class PromoCode
{
    public string ID { get; set; }
    public string Name { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ExpireDate { get; set; }
    public float Percentage { get; set; }
    public float MaxDiscount { get; set; }
    public ICollection<CustomerPromoCode> CustomersPromoCodes { get; set; }
}
