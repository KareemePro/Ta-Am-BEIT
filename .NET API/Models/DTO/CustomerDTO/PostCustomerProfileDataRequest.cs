namespace FoodDelivery.Models.DTO.CustomerDTO;

public record PostCustomerProfileDataRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PhoneNumber { get; init; }
    public Guid BuildingID { get; init; }
    public string? Image { get; init; }
}
