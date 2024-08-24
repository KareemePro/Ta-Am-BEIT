namespace FoodDelivery.Models.DominModels.Meals;

public class SideDish
{
    public Guid ID { get; set; }
    public string ChiefID { get; set; }
    public string Name { get; set; }
    public bool IsAvailable { get; set; }
    public string ThumbnailImage { get; set; }
    public string FullScreenImage { get; set; }
    public virtual Chief Chief { get; set; }
    public ICollection<SideDishOption> SideDishOptions { get; set;}
}
