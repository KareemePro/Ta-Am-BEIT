using System.Text.Json.Serialization;

namespace FoodDelivery.Models.DominModels.Auth;

public class AuthModel
{
    public List<string>? Roles { get; set; }
    public string? jwt { get; set; }
    public DateTime? ExpiresOn { get; set; }

    [JsonIgnore]
    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiration { get; set; }
}