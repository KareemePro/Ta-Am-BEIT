using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DominModels.Auth;

public class TokenRequestModel
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}