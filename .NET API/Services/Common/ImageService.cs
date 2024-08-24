using Firebase.Auth;
using Firebase.Storage;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.ImageDTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Runtime.CompilerServices;

namespace FoodDelivery.Services.Common;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    //private readonly string wwwRootPath;

    private const int FullScreenWidth = 900;
    private const int ThumbnailWidth = 300;


    public ImageService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        //wwwRootPath = _webHostEnvironment.WebRootPath;
    }
    public async Task<List<string>> Process(ImageInput image)
    {
        var tasks = new List<Task>();

        var ImageResult = await Image.LoadAsync(image.Content);

        string ThumbnailImage = "";
        string FullScreenImage = "";

        tasks.Add(Task.Run( async () =>
            {
                //var storagePath = Path.Combine(Directory.GetCurrentDirectory(), $"{image.Path}");


                //if (!Directory.Exists(storagePath))
                //{
                //    Directory.CreateDirectory(storagePath);
                //}

                FullScreenImage = await SavingImage(ImageResult, $"FullScreen_{image.FileName}", image.Path, FullScreenWidth);
                ThumbnailImage = await SavingImage(ImageResult, $"Thumbnail_{image.FileName}", image.Path, ThumbnailWidth);
            }));

        await Task.WhenAll(tasks);

        return [ThumbnailImage, FullScreenImage];
    }
    private async Task<string> SavingImage(Image image,string name, string path, int resizeWidth)
    {

        var width = image.Width;
        var height = image.Height;

        if (width > resizeWidth)
        {
            height = (int)((double)resizeWidth / width * height);
            width = resizeWidth;
        }
        image.Mutate(x => x.Resize(width, height));

        var momeryStream = new MemoryStream();

        //if (File.Exists($"{path}/{name}.jpg"))
        //{
        //    File.Delete($"{path}/{name}.jpg");
        //}

        //await image.SaveAsJpegAsync($"{path}/{name}.jpg", new JpegEncoder
        //{
        //    Quality = 90,
        //    SkipMetadata = true
        //});

        await image.SaveAsJpegAsync(momeryStream, new JpegEncoder
        {
            Quality = 90,
            SkipMetadata = true
        });


        return await UploadFiles(momeryStream, path , name);


    }

    public async Task<string> UploadFiles(MemoryStream stream, string path, string name)
    {
        stream.Position = 0;
        //using var stream = new MemoryStream();
        //await file.CopyToAsync(stream);
        var auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyBd16d14vL8hTQR3ufgrCkvyeBwjnN3koY"));
        var a = await auth.SignInWithEmailAndPasswordAsync("omar.1met@gmail.com", "123456zZ!");
        var cancellation = new CancellationTokenSource();

        var task = new FirebaseStorage(
            "fir-e4e25.appspot.com",
            new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                ThrowOnCancel = true
            })
            .Child(path)
            .Child(name+".jpg")
            .PutAsync(stream, cancellation.Token);

        task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");
        try
        {
            var downloadUrl = await task;
            Console.WriteLine($"Download link:\n{downloadUrl}");
            return downloadUrl;
        }
        catch (Exception ex)
        {
            return "";
        }
    }
    public async void UploadFiles(IFormFile file)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        var auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyBd16d14vL8hTQR3ufgrCkvyeBwjnN3koY"));
        var a = await auth.SignInWithEmailAndPasswordAsync("omar.1met@gmail.com", "123456zZ!");
        var cancellation = new CancellationTokenSource();

        var task = new FirebaseStorage(
            "fir-e4e25.appspot.com",
            new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                ThrowOnCancel = true
            })
            .Child("receipts")
            .Child(Guid.NewGuid().ToString())
            .PutAsync(stream, cancellation.Token);

        task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");
        try
        {
            var downloadUrl = await task;
            Console.WriteLine($"Download link:\n{downloadUrl}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    /*public string SaveImage(IFormFile request, string path, string filename)
    {
        var uploads = Path.Combine(wwwRootPath, path);
        var extension = Path.GetExtension(request.FileName);
        var fullfilename = $"{filename}{extension}";

        var filePath = Path.Combine(uploads, fullfilename);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            request.CopyTo(fileStream);
        }
        return $"{path}/{fullfilename}";

    }

    public bool DeleteImage(string filePath)
    {
        filePath = Path.Combine(wwwRootPath, filePath);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }
        return false;
    }*/
}
