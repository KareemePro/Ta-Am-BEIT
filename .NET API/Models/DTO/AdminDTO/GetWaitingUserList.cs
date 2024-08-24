namespace FoodDelivery.Models.DTO.AdminDTO;

public record GetWaitingUserList
{
    public string UserID { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string? PhoneNumber { get; init; }
    public DateTime? SignedUpDate { get; init; }
}
