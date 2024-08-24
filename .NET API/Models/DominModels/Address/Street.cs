using System.Text.Json.Serialization;

namespace FoodDelivery.Models.DominModels.Address;

public class Street
{
    public Guid ID { get; set; }

    public Guid DistrictID { get; set; }

    public string Name { get; set; }
    [JsonIgnore]
    public virtual District District { get; set; }
    [JsonIgnore]
    public ICollection<Building> Buildings { get; set; }
}
