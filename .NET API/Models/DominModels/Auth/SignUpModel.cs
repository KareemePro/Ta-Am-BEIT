using FoodDelivery.Models.Utility;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DominModels.Auth;

public class SignUpModel
{
    [StringLength(100)]
    public string FirstName { get; set; }

    [StringLength(100)]
    public string LastName { get; set; }

    [StringLength(128)]
    public string Email { get; set; }

    [StringLength(256)]
    public string Password { get; set; }
}
