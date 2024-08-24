using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.ImageDTO;

namespace FoodDelivery.Services.Common;

public interface IImageService
{
    //string SaveImage(IFormFile request, string path, string filename);

    //bool DeleteImage(string path);

    Task<List<string>> Process(ImageInput image);

    void UploadFiles(IFormFile file);


    //Task<ICollection<string>> SaveImages(ICollection<IFormFile> request, string path);
}
