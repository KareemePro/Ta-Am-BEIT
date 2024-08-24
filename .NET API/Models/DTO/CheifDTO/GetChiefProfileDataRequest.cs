namespace FoodDelivery.Models.DTO.CheifDTO;

public record GetChiefProfileDataRequest
{
    public string? ChiefID { get; init; }
    public Guid? DistrictID { get; init; }
    public Guid? StreetID { get; init; }
    public Guid? BuildingID { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Description { get; init; }
    public string? CoverImage { get; init; }
    public string? ChiefImage { get; init; }
    public string? HealthCertImage { get; init; }
    public string? PhoneNumber { get; init; }
    public string? FloorNumber { get; init; }
    public string? ApartmentNumber { get; init; }
    public string? GovernmentID { get; init; }
    public TimeOnly? StartTime { get; init; }
    public TimeOnly? CloseTime { get; init; }
}
