using FoodDelivery.Data;
using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.CheifDTO;
using FoodDelivery.Models.DTO.ImageDTO;
using FoodDelivery.Models.DTO.IngredientDTO;
using FoodDelivery.Models.DTO.MealDTO;
using FoodDelivery.Services.Addresses;
using FoodDelivery.Services.Common;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FoodDelivery.Services.Cheifs;

public class ChiefService(DBContext context, IImageService image) : IChiefService
{
    private readonly DBContext _context = context;

    private readonly IImageService _image = image;

    //public async Task<SingleResult<bool>> AddIngredients(List<UpsertIngredientRequest> request, Guid ChiefID)
    //{
    //    if (!await _context.Chiefs.AnyAsync(x => x.Id == ChiefID.ToString()))
    //        return SingleResult<bool>.Failure(["please try to log in again"], HttpStatusCode.NotFound);

    //    var NewChiefIngredients = request.Select(x => new ChiefIngredient()
    //    {
    //        ChiefID = ChiefID.ToString(),
    //        FoodIngredient = x.Ingredient,
    //        CostPerKilo = x.PricePerKilo
    //    }).ToList();

    //    var ExistingChiefIngredients = await _context.ChiefIngredients.Where(x => x.ChiefID == ChiefID.ToString()).ToListAsync();

    //    if (ExistingChiefIngredients == null)
    //    {
    //        await _context.AddAsync(NewChiefIngredients);
    //        await _context.SaveChangesAsync();
    //        return SingleResult<bool>.Success(true);
    //    }

    //    for (int i = NewChiefIngredients.Count - 1; i >= 0; i--)
    //    {
    //        if (NewChiefIngredients.Count < 1)
    //            break;

    //        var existingChiefIngredient = ExistingChiefIngredients.Where(x => x.FoodIngredient == NewChiefIngredients[i].FoodIngredient).FirstOrDefault();
    //        if (existingChiefIngredient == null)
    //            continue;

    //        existingChiefIngredient.CostPerKilo = NewChiefIngredients[i].CostPerKilo;
    //        NewChiefIngredients.RemoveAt(i);
    //    }
    //    _context.Update(ExistingChiefIngredients);
    //    await _context.AddAsync(NewChiefIngredients);
    //    await _context.SaveChangesAsync();
    //    return SingleResult<bool>.Success(true);

    //}
    //public async Task<SingleResult<bool>> AddIngredients(List<UpsertIngredientRequest> request, Guid ChiefID)
    //{
    //    var chiefExists = await _context.Chiefs.AnyAsync(x => x.Id == ChiefID.ToString());
    //    if (!chiefExists)
    //    {
    //        return SingleResult<bool>.Failure(["Please try to log in again."], HttpStatusCode.NotFound);
    //    }

    //    var existingChiefIngredients = await _context.ChiefIngredients
    //        .Where(x => x.ChiefID == ChiefID.ToString())
    //        .ToListAsync();

    //    foreach (var ingredientRequest in request)
    //    {
    //        var existingIngredient = existingChiefIngredients.FirstOrDefault(x => x.FoodIngredient == ingredientRequest.Ingredient);
    //        if (existingIngredient != null)
    //        {
    //            existingIngredient.CostPerKilo = ingredientRequest.PricePerKilo;
    //            existingIngredient.Visible = ingredientRequest.Delete;
    //        }
    //        else
    //        {
    //            existingChiefIngredients.Add(new ChiefIngredient
    //            {
    //                ChiefID = ChiefID.ToString(),
    //                FoodIngredient = ingredientRequest.Ingredient,
    //                CostPerKilo = ingredientRequest.PricePerKilo,
    //                Visible = true
    //            });
    //        }
    //    }

    //    _context.UpdateRange(existingChiefIngredients);
    //    await _context.SaveChangesAsync();

    //    return SingleResult<bool>.Success(true);
    //}

    public async Task<SingleResult<bool>> AddIngredients(List<UpsertIngredientRequest> request, Guid ChiefID)
    {
        var chiefExists = await _context.Chiefs.AnyAsync(x => x.Id == ChiefID.ToString());
        if (!chiefExists)
        {
            return SingleResult<bool>.Failure(["Please try to log in again."], HttpStatusCode.NotFound);
        }

        var existingChiefIngredients = await _context.ChiefIngredients
            .Where(x => x.ChiefID == ChiefID.ToString())
            .ToListAsync();

        var ingredientsToUpdate = new List<ChiefIngredient>();
        var ingredientsToAdd = new List<ChiefIngredient>();

        foreach (var ingredientRequest in request)
        {
            var existingIngredient = existingChiefIngredients.FirstOrDefault(x => x.FoodIngredient == ingredientRequest.Ingredient);
            if (existingIngredient != null)
            {
                existingIngredient.CostPerKilo = ingredientRequest.PricePerKilo;
                existingIngredient.Visible = true;
                ingredientsToUpdate.Add(existingIngredient);
            }
            else
            {
                ingredientsToAdd.Add(new ChiefIngredient
                {
                    ChiefID = ChiefID.ToString(),
                    FoodIngredient = ingredientRequest.Ingredient,
                    CostPerKilo = ingredientRequest.PricePerKilo,
                    Visible = true
                });
            }
        }

        // Update existing ingredients
        _context.UpdateRange(ingredientsToUpdate);

        // Add new ingredients
        await _context.AddRangeAsync(ingredientsToAdd);

        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }

    public async Task<SingleResult<GetChiefMealsRequest>> GetChiefMeals(string ChiefID)
    {
        TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);

        var Chief = await _context.Chiefs.Where(x => x.Id == ChiefID).Select(chief => new GetChiefMealsRequest()
        {
            ChiefID = chief.Id,
            ChiefName = chief.FirstName + " " + chief.LastName,
            Description = chief.Description ?? " ",
            CoverImage = chief.CoverImage == null ? "https://currystory.com.au/images/offers1.jpg" : chief.CoverImage,
            PrfileImage = chief.ChiefFullScreenImage == null ? "https://i.pinimg.com/736x/ff/6b/48/ff6b489e46c3e4133502c31e078085e1.jpg" : chief.ChiefFullScreenImage,
            IsOnline = chief.OpeningTime < currentTime && chief.ClosingTime > currentTime,
            OrdersDone = chief.Meals.SelectMany(m => m.MealOptions).SelectMany(o => o.OrderItems).Count(),
            ReviewCount = chief.Meals.SelectMany(m => m.MealReviews).Count(),//chief.Meals.Sum(x => x.MealReviews.Count()),
            Rating = chief.Meals.SelectMany(m => m.MealReviews).Any() ? chief.Meals.SelectMany(m => m.MealReviews).Average(o => o.Rating) : 0,//chief.Meals.SelectMany(m => m.MealReviews).Select(o => o.Rating).DefaultIfEmpty(0).Average(),
            //chief.Meals.Sum(x => x.MealReviews.Average(y => y.Rating)),
            Meals = chief.Meals.Where(x => x.IsAvailable == true).Select(x => new GetMealRequest()
            {
                ChiefID = x.ChiefID,
                ChiefName = x.Chief.FirstName+ " " + x.Chief.LastName,
                ChiefImage = x.Chief.UserImage ?? "https://currystory.com.au/images/offers1.jpg",
                ChiefDescription = x.Chief.Description ?? "",
                ChiefOrderCount = 0,
                MealID = x.ID,
                Title = x.Name,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                Rating = x.MealReviews.Any() ? x.MealReviews.Average(x => x.Rating) : 0,
                ReviewCount = x.MealReviews.Any() ? x.MealReviews.Count : 0,
                MealCategory = x.MealCategory,
                MealSpiceLevel = x.MealSpiceLevel,
                MealStyle = x.MealStyle,
                MealTags = x.MealTags.Select(x => x.Tag).ToArray(),
                GetMealOptionsRequest = x.MealOptions.Select(opt => new GetMealOptionRequest()
                {
                    MealOptionID = opt.ID,
                    MealSizeOption = opt.MealSizeOption,
                    IsAvailable = opt.IsAvailable,
                    Quantity = (int)(opt.AvailableQuantity == null ? 0 : opt.AvailableQuantity),
                    SaveQuantity = (opt.DailyQuantity != null),
                    Price = opt.Price,
                    FullScreenImage = opt.FullScreenImage,
                    ThumbnailImage = opt.ThumbnailImage,
                    GetMealSideDishesRequest = opt.MealSideDishes.Select(x => new GetMealSideDishRequest()
                    {
                        MealSideDishID = x.ID,
                        IsFree = x.IsFree,
                        IsTopping = x.IsTopping,
                        GetMealSideDishOptionsRequest = x.MealSideDishOptions.Select(y => new GetMealSideDishOptionRequest()
                        {
                            SideDishID = y.SideDishID,
                            SideDishSizeOption = y.SideDishSizeOption,
                            Name = y.SideDishOption.SideDish.Name,
                            Price = y.SideDishOption.Price,
                            Quantity = y.SideDishOption.Quantity,
                        }).ToArray()
                    }).ToArray()
                }).ToArray(),
            }).ToArray()
        }).FirstOrDefaultAsync();

        if (Chief == null)
            return SingleResult<GetChiefMealsRequest>.Failure(["this chief does not exist"], HttpStatusCode.NotFound);

        return SingleResult<GetChiefMealsRequest>.Success(Chief);
    }

    public async Task<SingleResult<GetChiefProfileDataRequest>> GetChiefProfileData(Guid ChiefID)
    {
        var ChiefData = await _context.Chiefs.Include(x => x.Building)
            .ThenInclude(x => x.Street).Where(x => x.Id == ChiefID.ToString()).Select(x => new GetChiefProfileDataRequest()
            {
                ChiefID = x.Id,
                DistrictID = x.Building.Street.DistrictID,
                StreetID = x.Building.StreetID,
                BuildingID = x.BuildingID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Description = x.Description,
                PhoneNumber = x.PhoneNumber,
                CoverImage = x.CoverImage,
                ChiefImage = x.ChiefFullScreenImage,
                HealthCertImage = x.HealthCertImage,
                FloorNumber = x.FloorNumber,
                ApartmentNumber = x.ApartmentNumber,
                StartTime = x.OpeningTime,
                CloseTime = x.ClosingTime,
                GovernmentID = x.GovernmentID
            }).FirstOrDefaultAsync();

        if (ChiefData == null)
            return SingleResult<GetChiefProfileDataRequest>.Failure(["please try to refreash your page"], HttpStatusCode.NotFound);

        return SingleResult<GetChiefProfileDataRequest>.Success(ChiefData);

    }

    public async Task<ListResult<GetChiefIngredientRequest>> GetIngredients(Guid ChiefID)
    {
        var chiefExists = await _context.Chiefs.AnyAsync(x => x.Id == ChiefID.ToString());
        if (!chiefExists)
        {
            return ListResult<GetChiefIngredientRequest>.Failure(["Please try to log in again."], HttpStatusCode.NotFound);
        }

        var Ingredients = await _context.ChiefIngredients.Where(x => x.ChiefID == ChiefID.ToString() && x.Visible == true).Select(x => new GetChiefIngredientRequest()
        {
            PricePerKilo = x.CostPerKilo,
            Ingredient = x.FoodIngredient
        }).ToArrayAsync();

        return ListResult<GetChiefIngredientRequest>.Success(Ingredients);
    }

    public async Task<SingleResult<bool>> RemoveIngredient(FoodIngredient ingredientToBeRemoved, Guid ChiefID)
    {
        var ingredient = await _context.ChiefIngredients.AsTracking().FirstOrDefaultAsync(x => x.FoodIngredient == ingredientToBeRemoved && x.ChiefID == ChiefID.ToString());
        if (ingredient == null)
            return SingleResult<bool>.Failure(["please try refreshing your page"], HttpStatusCode.NotFound);
        ingredient.Visible = false;
        await _context.SaveChangesAsync();
        return SingleResult<bool>.Success(true);

    }

    public async Task<SingleResult<bool>> UpsertChiefData(UpsertChiefDataRequest request, Guid ChiefID, byte[]? ChiefImage, byte[]? CoverImage, byte[]? HealthCertImage)
    {
        var Chief = await _context.Chiefs.AsTracking().FirstOrDefaultAsync(x => x.Id == ChiefID.ToString());

        if (Chief == null)
            return SingleResult<bool>.Failure(["please try to log in again"], HttpStatusCode.NotFound);

        Chief.FirstName = request.FirstName ?? Chief.FirstName;
        Chief.LastName = request.LastName ?? Chief.LastName;
        Chief.OpeningTime = request.StartTime ?? Chief.OpeningTime;
        Chief.ClosingTime = request.CloseTime ?? Chief.ClosingTime;
        Chief.Description = request.Description ?? Chief.Description;
        Chief.PhoneNumber = request.PhoneNumber ?? Chief.PhoneNumber;
        Chief.FloorNumber = request.FloorNumber ?? Chief.FloorNumber;
        Chief.ApartmentNumber = request.ApartmentNumber ?? Chief.ApartmentNumber;
        Chief.GovernmentID = request.GovernmentID ?? Chief.GovernmentID;

        if (await _context.Buildings.AnyAsync(x => x.ID == request.BuildingID)) Chief.BuildingID = request.BuildingID;

        var Images = new List<string>();
        Images = ChiefImage != null ? Images = request.ChiefImage != null ? await _image.Process(new ImageInput() { Content = new MemoryStream(ChiefImage), FileName = $"{Chief.Id}", Path = "Images/User" }) : [] : [];
        Chief.UserImage = Images != null && Images.Count > 0 ? Images[0] : Chief.UserImage;
        Chief.ChiefFullScreenImage = Images != null && Images.Count > 0 ? Images[1] : Chief.ChiefFullScreenImage;
        Images?.Clear();

        Images = HealthCertImage != null ? Images = request.HealthCertImage != null ? await _image.Process(new ImageInput() { Content = new MemoryStream(HealthCertImage), FileName = $"{Chief.Id}", Path = "Images/HealthCert" }) : [] : [];
        Chief.HealthCertImage = Images != null && Images.Count > 0 ? Images[1] : Chief.HealthCertImage;
        Images?.Clear();

        Images = CoverImage != null ? Images = request.CoverImage != null ? await _image.Process(new ImageInput() { Content = new MemoryStream(CoverImage), FileName = $"{Chief.Id}", Path = "Images/ChiefCover" }) : [] : [];
        Chief.CoverImage = Images != null && Images.Count > 0 ? Images[1] : Chief.CoverImage;
        Images?.Clear();

        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);

    }
}
