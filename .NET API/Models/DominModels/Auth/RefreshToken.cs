using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Models.DominModels.Auth;

[Owned]
public class RefreshToken
{
    public string? Token { get; set; }
    public DateTime TokenExpiresOn { get; set; }   
    
    public virtual User User { get; set; }
    /*public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresOn { get; set; }
    public bool IsExpired  => DateTime.UtcNow >= ExpiresOn;
    public DateTime CreatedOn { get; set; }
    public DateTime? RevokedOn { get; set; }
    public bool IsActive => RevokedOn == null && !IsExpired;*/
}