using Azure.Core;
using FoodDelivery.Data;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.ImageDTO;
using FoodDelivery.Models.DTO.MealReviewDTO;
using FoodDelivery.Services.Common;
using FoodDelivery.Settings;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FoodDelivery.Services.MealReviews;

public class MealReviewService : IMealReviewService
{
    private readonly DBContext _context;
    private readonly IImageService _imageService;
    public MealReviewService(DBContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }

    public async Task<ListResult<GetMealReviewRequest>> GetReviewByCustomer(string CustomerID)
    {
        var result = await _context.MealReviews.Where(x => x.CustomerID == CustomerID).Select(x => new GetMealReviewRequest(x.MealID, x.CustomerID, x.Title, x.Description , x.Rating , x.FullScreenImage, x.ThumbnailImage,x.CreatedAt,x.UpdatedAt)).ToArrayAsync();
        if (result != null)
        {
            return ListResult<GetMealReviewRequest>.Success(result);
        }
        return ListResult<GetMealReviewRequest>.Failure(["This customer has no reviews"], HttpStatusCode.NotFound);
    }

    public async Task<bool> UpsertMealReview(UpsertMealReviewRequest request, string CustomerID)
    {
        var Meal = await _context.Meals.FirstOrDefaultAsync(x => x.ID == request.MealID);
        if (Meal == null)
            return false;


        if (await _context.Customers.AnyAsync(x => x.Id == CustomerID))
        {
            var MealReview = await _context.MealReviews.FirstOrDefaultAsync(x => x.MealID == request.MealID && x.CustomerID == CustomerID);

            if(MealReview != null)
            {
                MealReview.Rating = request.Rating;
                MealReview.Title = request.Text;
                MealReview.UpdatedAt = DateTime.Now;
                if (request.ReviewImage != null)
                {
                    var Images = await _imageService.Process(new ImageInput { Content = request.ReviewImage.OpenReadStream(), FileName = Guid.NewGuid().ToString(), Path = "Images/MealReview" });
                    MealReview.ThumbnailImage = Images[0] ?? "";
                    MealReview.FullScreenImage = Images[1] ?? "";
                }
                _context.Update(MealReview);
            }
            else
            {
                var CurrentDate = DateTime.Now;

                MealReview mealReview = new()
                {
                    CustomerID = CustomerID,
                    MealID = request.MealID,
                    Rating = request.Rating,
                    Title = request.Text,
                    CreatedAt = CurrentDate,
                    UpdatedAt = CurrentDate

                };
                if (request.ReviewImage != null)
                {
                    var Images = await _imageService.Process(new ImageInput { Content = request.ReviewImage.OpenReadStream(), FileName = Guid.NewGuid().ToString(), Path = "Images/MealReview" });
                    MealReview.ThumbnailImage = Images[0] ?? "";
                    MealReview.FullScreenImage = Images[1] ?? "";
                }
                await _context.AddAsync(mealReview);

            }

            await _context.SaveChangesAsync();
            Meal.Rating = await _context.MealReviews.Where(x => x.MealID == Meal.ID).AverageAsync(x => x.Rating);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    //public async Task<bool> EditMealReview(UpsertMealReviewRequest request)
    //{
    //    var mealReview = await _context.MealReviews.AsTracking().Where(x => x.MealID == request.MealID && x.CustomerID == request.CustomerID).FirstOrDefaultAsync();
        
    //    if(mealReview != null)
    //    {
    //        mealReview.MealID = request.MealID;
    //        mealReview.CustomerID = request.CustomerID;
    //        mealReview.Title = request.Title;
    //        mealReview.Description = request.Description;
    //        mealReview.Rating = request.Rating;
    //        mealReview.UpdatedAt = DateTime.Now;
    //        if (request.ReviewImage != null)
    //        {
    //            //if(mealReview.ReviewImage != null) mealReview.ReviewImage = await _imageService.Process(new ImageInput { Content = request.ReviewImage.OpenReadStream(), FileName = mealReview.ReviewImage.Replace(".jpg","").Replace("Images/MealReview/", ""), Path = "Images/MealReview" });
    //            //else mealReview.ReviewImage = await _imageService.Process(new ImageInput { Content = request.ReviewImage.OpenReadStream(), FileName = Guid.NewGuid().ToString(), Path = "Images/MealReview" });
    //            var Images = await _imageService.Process(new ImageInput { Content = request.ReviewImage.OpenReadStream(), FileName = Guid.NewGuid().ToString(), Path = "Images/MealReview" });
    //            mealReview.ThumbnailImage = Images[0] ?? "";
    //            mealReview.FullScreenImage = Images[1] ?? "";
    //        }
    //        await _context.SaveChangesAsync();
    //        var Meal = await _context.Meals.FirstAsync(x => x.ID == request.MealID);
    //        Meal.Rating = await _context.MealReviews.Where(x => x.MealID == Meal.ID).AverageAsync(x => x.Rating);
    //        _context.Update(Meal);
    //        await _context.SaveChangesAsync();
    //        return true;
    //    }
    //    return false;// SingleResult<bool>.Failure(["Wrong customer or meal id"]);
    //}

    public async Task<bool> DeleteMealReview(Guid MealID, string CustomerID)
    {
        var mealReview = await _context.MealReviews.Where(x => x.MealID == MealID && x.CustomerID == CustomerID).FirstOrDefaultAsync();
        if (mealReview != null)
        {
            _context.MealReviews.Remove(mealReview);
            await _context.SaveChangesAsync();
            var Meal = await _context.Meals.FirstAsync(x => x.ID == MealID);
            Meal.Rating = await _context.MealReviews.Where(x => x.MealID == Meal.ID).AverageAsync(x => x.Rating);
            _context.Update(Meal);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<SingleResult<GetMealReviewsRequest>> GetMealReviews(Guid MealID)
    {

        var meal = await _context.Meals.FirstOrDefaultAsync(m => m.ID == MealID);
        if (meal != null)
        {
            var groupedRatings = await _context.MealReviews
                .Where(x => x.MealID == MealID)
                .GroupBy(x => x.Rating)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            var reviews = await _context.MealReviews
                .Where(x => x.MealID == MealID)
                .Select(x => new GetMealReview()
                {
                    CustomerName = x.Customer.FirstName + " " + x.Customer.LastName,
                    CustomerImage = x.Customer.UserImage ?? "https://static-00.iconduck.com/assets.00/profile-default-icon-1024x1023-4u5mrj2v.png",
                    ReviewText = x.Title,
                    Rating = (int)x.Rating,
                    ReviewDate = x.CreatedAt
                }).ToArrayAsync();

            var mealReviews = new GetMealReviewsRequest()
            {
                MealID = MealID,
                MealName = meal.Name,
                Rating = meal.Rating,
                FiveStarCount = groupedRatings.TryGetValue(5, out int value5) ? value5 : 0,
                FourStarCount = groupedRatings.TryGetValue(4, out int value4) ? value4 : 0,
                ThreeStarCount = groupedRatings.TryGetValue(3, out int value3) ? value3 : 0,
                TwoStarCount = groupedRatings.TryGetValue(2, out int value2) ? value2 : 0,
                OneStarCount = groupedRatings.TryGetValue(1, out int value1) ? value1 : 0,
                MealReviews = reviews
            };

            return SingleResult<GetMealReviewsRequest>.Success(mealReviews);
        }
        return SingleResult<GetMealReviewsRequest>.Failure(["this meal does not exist"], HttpStatusCode.NotFound);

    }
}