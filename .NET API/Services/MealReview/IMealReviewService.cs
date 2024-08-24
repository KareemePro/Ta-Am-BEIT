using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.AddressDTO;
using FoodDelivery.Models.DTO.MealReviewDTO;
using FoodDelivery.Services.Common;

namespace FoodDelivery.Services.MealReviews;

public interface IMealReviewService
{
    Task<SingleResult<GetMealReviewsRequest>> GetMealReviews(Guid MealID);
    Task<ListResult<GetMealReviewRequest>> GetReviewByCustomer(string Id);
    Task<bool> UpsertMealReview(UpsertMealReviewRequest request, string CustomerID);
    //Task<bool> EditMealReview(UpsertMealReviewRequest request);
    Task<bool> DeleteMealReview(Guid mealId, string customerId);
}
