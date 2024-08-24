namespace FoodDelivery.Models.DTO.MealReviewDTO;

public record GetMealReviewsRequest
{
    public Guid MealID { get; init; }
    public float Rating { get; init; }
    public string MealName { get; init;}
    public int FiveStarCount { get; init; }
    public int FourStarCount { get; init; }
    public int ThreeStarCount { get; init; }
    public int TwoStarCount { get; init; }
    public int OneStarCount { get; init; }
    public ICollection<GetMealReview> MealReviews { get; init; }
}

public record GetMealReview
{
    public string CustomerName { get; init; }
    public string CustomerImage { get; init; }
    public string ReviewText { get; init; }
    public int Rating { get; init; }
    public DateTime ReviewDate { get; init; }
}
