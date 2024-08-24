namespace FoodDelivery.Models.DTO.ImageDTO;

public class ImageInput
{
    public string FileName { get; set; }
    public string Type { get; set; }
    public string Path { get; set; }
    public Stream Content { get; set; }

}
