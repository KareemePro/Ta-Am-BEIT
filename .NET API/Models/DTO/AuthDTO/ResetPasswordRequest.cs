using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.AuthDTO;

public record ResetPasswordRequest
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
}
