using System.Text.Json.Serialization;

namespace FoodDelivery.Models.DominModels.Address;

public class Building
{
    public Guid ID { get; set; }

    public Guid StreetID { get; set; }

    public string Name { get; set; }
    [JsonIgnore]
    public virtual Street Street { get; set; }
    public ICollection<User> Users { get; set; }
}
