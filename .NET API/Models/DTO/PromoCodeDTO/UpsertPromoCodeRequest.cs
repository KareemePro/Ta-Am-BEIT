using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.PromoCodeDTO;

public class UpsertPromoCodeRequest
{
    public string ID { get; set; }
    public string Name { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ExpireDate { get; set; }
    [Range(0,.99)]
    public float Percentage { get; set; }
    public float MaxDiscount { get; set; }
}
