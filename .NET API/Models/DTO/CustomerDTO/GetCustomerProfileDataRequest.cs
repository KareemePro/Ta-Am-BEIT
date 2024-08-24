namespace FoodDelivery.Models.DTO.CustomerDTO;

public class GetCustomerProfileDataRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PhoneNumber { get; init; }
    public Guid? DistrictID { get; init; }
    public Guid? StreetID { get; init; }
    public Guid? BuildingID { get; init; }
    public string? Image { get; init; }
}
