using System.Text.Json.Serialization;

namespace FoodDelivery.Models.DominModels.Address;

public class Governorate
{
    public Guid ID { get; set; }

    public string Name { get; set; }
    [JsonIgnore]
    public ICollection<District> Districts { get; set; }
}
