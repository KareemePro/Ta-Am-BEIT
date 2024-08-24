using FoodDelivery.Models.Utility;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FoodDelivery.Models.DominModels.Auth;

public class ChiefSignUpModel : SignUpModel
{

    public string PhoneNumber { get; set; }

    public Guid BuildingID { get; set; }

    [MaxFileSize(10 * 1024 * 1024)]
    [AllowedExtension(".jpg,.png,.jpeg")]
    public IFormFile? Image { get; set; }
}
