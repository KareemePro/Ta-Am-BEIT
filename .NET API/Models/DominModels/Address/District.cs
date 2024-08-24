using System.Text.Json.Serialization;

namespace FoodDelivery.Models.DominModels.Address;

public class District
{
    public Guid ID { get; set; }

    public Guid GovernorateID { get; set; }

    public string Name { get; set; }
    [JsonIgnore]
    public virtual Governorate Governorate { get; set; }
    [JsonIgnore]
    public ICollection<Street> Streets { get; set; }
}
