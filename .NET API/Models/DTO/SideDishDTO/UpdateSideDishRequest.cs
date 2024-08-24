namespace FoodDelivery.Models.DTO.SideDishDTO;

public record UpdateSideDishRequest
{
    public Guid ID { get; init; }
    public string? Name { get; init; }
    public string? Image { get; init; }
}
