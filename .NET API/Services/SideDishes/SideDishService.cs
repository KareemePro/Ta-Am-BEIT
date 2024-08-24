using FoodDelivery.Data;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DominModels.Meals;
using FoodDelivery.Models.DTO.ImageDTO;
using FoodDelivery.Models.DTO.SideDishDTO;
using FoodDelivery.Services.Common;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FoodDelivery.Services.SideDishes;

public class SideDishService(DBContext context, IImageService image) : ISideDishService
{
    private readonly DBContext _context = context;

    private readonly IImageService _image = image;

    public async Task<SingleResult<Guid>> AddSideDish(CreateSideDishRequest request, Guid ChiefID, byte[] image)
    {
        SideDish SideDish = new()
        {
            ID = Guid.NewGuid(),
            ChiefID = ChiefID.ToString(),
            IsAvailable = true,
            Name = request.Name,
        };
        var imagesURL = await _image.Process(new ImageInput() { Content = new MemoryStream(image), FileName = $"{SideDish.ID}", Path = "Images/SideDish" });
        SideDish.ThumbnailImage = imagesURL[0];
        SideDish.FullScreenImage = imagesURL[1];

        await _context.AddAsync(SideDish);
        await _context.SaveChangesAsync();

        return SingleResult<Guid>.Success(SideDish.ID);
    }

    public async Task<SingleResult<bool>> AddSideDishOption(CreateSideDishOptionRequest request, Guid ChiefID)
    {
        if (!await _context.SideDishes.AnyAsync(x => x.ID == request.SideDishID))
            return SingleResult<bool>.Failure(["this side dish does not exist"]);

        SideDishOption SideDishOption = new()
        {
            SideDishID = request.SideDishID,
            SideDishSizeOption = request.SideDishSizeOption,
            Price = request.Price,
            Quantity = request.Quantity,
        };

        await _context.AddAsync(SideDishOption);
        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }

    public async Task<SingleResult<bool>> UpdateSideDish(UpdateSideDishRequest request, Guid ChiefID, byte[] image)
    {
        var SideDish = await _context.SideDishes.AsTracking().FirstOrDefaultAsync(x => x.ID == request.ID);

        if (SideDish == null)
            return SingleResult<bool>.Failure(["this side dish does not exist"]);

        SideDish.Name = request.Name ?? SideDish.Name;
        var imagesURL = new List<string>();
        imagesURL = image != null ? imagesURL = request.Image != null ? await _image.Process(new ImageInput() { Content = new MemoryStream(image), FileName = $"{SideDish.ID}", Path = "Images/SideDish" }) : [] : [];
        SideDish.ThumbnailImage = imagesURL != null && imagesURL.Count > 0 ? imagesURL[0] : SideDish.ThumbnailImage;
        SideDish.FullScreenImage = imagesURL != null && imagesURL.Count > 0 ? imagesURL[1] : SideDish.FullScreenImage;


        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }

    public async Task<SingleResult<bool>> UpdateSideOptionDish(UpdateSideDishOptionRequest request, Guid ChiefID)
    {
        var SideDishOption = await _context.SideDishOptions.AsTracking().FirstOrDefaultAsync(x => x.SideDishID == request.SideDishID && x.SideDishSizeOption == request.SideDishSizeOption);

        if (SideDishOption == null)
            return SingleResult<bool>.Failure(["this side dish option does not exist"]);

        SideDishOption.Quantity = request.Quantity ?? SideDishOption.Quantity;
        SideDishOption.Price = request.Price ?? SideDishOption.Price;

        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }
    public async Task<SingleResult<GetSideDishRequest>> GetSideDish(Guid SideDishID)
    {
        var SideDish = await _context.SideDishes.Where(x => x.ID == SideDishID).Select(x => new GetSideDishRequest()
        {
            SideDishID = x.ID,
            Name = x.Name,
            FullScreenImage = x.FullScreenImage,
            ThumbnailImage = x.ThumbnailImage,
            GetSideDishOptions = x.SideDishOptions.Select(y => new GetSideDishOption()
            {
                Price = y.Price,
                AvailableQuantity = y.Quantity,
                SideDishSizeOption = y.SideDishSizeOption
            }).ToArray()
        }).FirstOrDefaultAsync();

        if (SideDish == null)
            return SingleResult<GetSideDishRequest>.Failure(["this side dish does not exist"], HttpStatusCode.NotFound);
        return SingleResult<GetSideDishRequest>.Success(SideDish);
    }
    public async Task<ListResult<GetSideDishRequest>> GetChiefSideDishes(Guid ChiefID)
    {
        var SideDishes = await _context.SideDishes.Where(x => x.ChiefID == ChiefID.ToString() && x.IsAvailable == true).Select(x => new GetSideDishRequest()
        {
            SideDishID = x.ID,
            Name = x.Name,
            FullScreenImage = x.FullScreenImage,
            ThumbnailImage = x.ThumbnailImage,
            GetSideDishOptions = x.SideDishOptions.Select(y => new GetSideDishOption()
            {
                Price = y.Price,
                AvailableQuantity = y.Quantity,
                SideDishSizeOption = y.SideDishSizeOption
            }).ToArray()
        }).ToListAsync();

        if (SideDishes == null)
            return ListResult<GetSideDishRequest>.Failure(["this chief has no side dishes"], HttpStatusCode.NotFound);
        return ListResult<GetSideDishRequest>.Success(SideDishes);
    }

    public async Task<ListResult<GetSideDishOptionRequest>> GetChiefSideDishOptions(Guid ChiefID)
    {
        var SideDishOptions = await _context.SideDishOptions.Where(x => x.SideDish.ChiefID == ChiefID.ToString()).Select(x => new GetSideDishOptionRequest()
        {
            SideDishID = x.SideDish.ID,
            SideDishSizeOption = x.SideDishSizeOption,
            Name = x.SideDish.Name,
            FullScreenImage = x.SideDish.FullScreenImage,
            ThumbnailImage = x.SideDish.ThumbnailImage,
            Price = x.Price,
            AvailableQuantity = x.Quantity,
        }).ToListAsync();

        if (SideDishOptions == null)
            return ListResult<GetSideDishOptionRequest>.Failure(["this chief has no side dishes"], HttpStatusCode.NotFound);
        return ListResult<GetSideDishOptionRequest>.Success(SideDishOptions);
    }

    public async Task<SingleResult<bool>> DisableSideDish(Guid SideDishID, Guid ChiefID)
    {
        var SideDishe = await _context.SideDishes.AsTracking().FirstOrDefaultAsync(x => x.ID == SideDishID && x.ChiefID == ChiefID.ToString());
        if (SideDishe != null)
        {
            SideDishe.IsAvailable = false;
            await _context.SaveChangesAsync();
            return SingleResult<bool>.Success(true);
        }
        return SingleResult<bool>.Failure(["please try refreashing your page and try again"], HttpStatusCode.Accepted);
    }
}
