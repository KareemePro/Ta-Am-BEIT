using FoodDelivery.Models.DTO.ImageDTO;
using FoodDelivery.Services.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]

public class ImageController : ControllerBase
{
    private readonly IImageService _image;

    public ImageController(IImageService image)
    {
        _image = image;
    }
    [HttpPost]

    public async Task <IActionResult> imageupload(IFormFile file)
    {
        //var imageinput = new ImageInput()
        //{
        //    FileName = file.FileName,
        //    Content = file.OpenReadStream(),
        //    Path = "Images" 
        //};


        
        _image.UploadFiles(file);
        return Ok('g');
    }

    //[HttpGet]
    //public IActionResult GetPdf()
    //{
    //    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

    //    // Create a new PDF document
    //    PdfDocument document = new PdfDocument();
    //    // Create an empty page
    //    PdfPage page = document.AddPage();

    //    // Get an XGraphics object for drawing
    //    XGraphics gfx = XGraphics.FromPdfPage(page);

    //    // Create a font
    //    XFont font = new XFont();

    //    // Draw the text
    //    gfx.DrawString("Hello, World!", font, XBrushes.Black,
    //    new XRect(0, 0, page.Width, page.Height),
    //    XStringFormats.Center);

    //    // Save the document to a MemoryStream
    //    MemoryStream stream = new MemoryStream();
    //    document.Save(stream, false);

    //    // Reset the position of the MemoryStream to the beginning
    //    stream.Position = 0;

    //    // Return the PDF as a file
    //    return new FileStreamResult(stream, "application/pdf");
    //}

}
