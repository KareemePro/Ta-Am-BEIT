using FoodDelivery.Models.DominModels.Address;
using FoodDelivery.Models.DominModels.Auth;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FoodDelivery.Models.DominModels;

public class User : IdentityUser
{
    [Required(ErrorMessage = "Please enter your first name")]
    [MinLength(3, ErrorMessage = "A first name must be at least 3 letters")]
    [MaxLength(15, ErrorMessage = "A first must be not more than 15 letters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter your first name in English letters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your last name")]
    [MinLength(3, ErrorMessage = "A last name must be at least 3 letters")]
    [MaxLength(15, ErrorMessage = "A last name must be not more than 15 letters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter your last name in English letters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    public override string? UserName { get; set; }

    public DateTime? SignupDate { get; set; }


    [Display(Name = "Profile Image")]
    public string? UserImage { get; set; }

    public Guid? BuildingID { get; set; }

    public bool IsEnabled { get; set; }

    public virtual Building Building { get; set; }

    public string? RefreshToken { get; set; }
    public bool IsRefreshTokenRevoked { get; set; }
    public DateTime? RefreshTokenExpiresOn { get; set; }
}
