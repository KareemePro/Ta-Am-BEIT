namespace FoodDelivery.Models.DTO.MealReviewDTO;

public record GetMealReviewRequest(Guid MealId, string CustomerID, string Title, string Description, float Rating, string FullScreenImage, string ThumbnailImage, DateTime CreatedAt, DateTime UpdatedAt);
