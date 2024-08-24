namespace FoodDelivery.Models.DTO.SideDishDTO;

public record CreateSideDishRequest
{
    public string Name { get; init; }
    public string Image { get; init; }
}
