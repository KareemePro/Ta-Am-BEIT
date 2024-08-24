using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.MealReviewDTO;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO.MealDTO;

public record GetMealRequest
{
    /*public GetMealRequest
        (Guid MealID,
        string ChiefID,
        string ChiefName,
        string ChiefImage,
        string MealName,
        string Description,
        float? Rating,
        //ICollection<string> MealImages,
        ICollection<GetMealTagRequest> MealTags,
        ICollection<GetMealOptionRequest> GetMealOptionsRequest)
    {
        this.MealID = MealID;
        this.ChiefID = ChiefID;
        this.ChiefName = ChiefName;
        this.ChiefImage = ChiefImage;
        this.MealName = MealName;
        this.Description = Description;
        this.Rating = Rating;
        //this.MealImages = MealImages;
        this.MealTags = MealTags;
        this.GetMealOptionsRequest = GetMealOptionsRequest;
    }*/
    public GetMealRequest()
    {

    }


    public Guid MealID { get; init; }
    public string ChiefID { get; init; }
    public string ChiefName { get; init; }
    public string ChiefDescription { get; init; }
    public string ChiefImage { get; init; }
    public int ChiefOrderCount { get; init; }
    public DateTime CreatedDate { get; init; }  
    public MealSpiceLevel MealSpiceLevel { get; init; }
    public MealCategory MealCategory { get; init; }
    public MealStyle MealStyle { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public float Rating { get; init; }
    public int ReviewCount { get; init; }
    //public ICollection<string> MealImages { get; init; }
    public ICollection<Enums.MealTag> MealTags { get; init; }
    public ICollection<GetMealOptionRequest> GetMealOptionsRequest { get; init; }
    public ICollection<GetMealReview>? GetMealReviewsRequest { get; init; }
    public ICollection<GetMealRequest>? GetSimilarMeals { get; set; }

}




public record GetMealOptionRequest
{
    public GetMealOptionRequest() { }

    public Guid MealOptionID { get; init; }
    public MealSizeOption MealSizeOption { get; init; }
    //public string SizeOptionName { get; init; }
    public bool IsAvailable { get; init; }
    public bool SaveQuantity { get; init; }
    public int Quantity { get; init; }
    public float Price { get; init; }
    public string ThumbnailImage { get; init; }
    public string FullScreenImage { get; init; }
    public ICollection<GetMealSideDishRequest> GetMealSideDishesRequest { get; init; }
    public ICollection<UsedIngredient>? UsedIngredients { get; init; }

}

public record GetMealSideDishRequest
{
    public Guid MealSideDishID { get; init; }
    public bool IsFree { get; init; }
    public bool IsTopping { get; init; }
    public ICollection<GetMealSideDishOptionRequest> GetMealSideDishOptionsRequest { get; init; }
    public GetMealSideDishRequest() { }

}

public record GetMealSideDishOptionRequest
{
    public Guid SideDishID { get; init; }
    public MealSizeOption SideDishSizeOption { get; init; }
    public string Name { get; init; }
    public float Price { get; init; }
    public int Quantity { get; init; }
    public GetMealSideDishOptionRequest() { }
}

public record UsedIngredient
{
    public FoodIngredient Ingredient { get; init; }
    public int AmountInGrams { get; init; }
}



//public record GetMealTagRequest
//{

//    public GetMealTagRequest() { }
//    public GetMealTagRequest(Guid TagID, string TagName) 
//    {
//        this.TagID = TagID;
//        this.TagName = TagName;
//    }
//    public Guid TagID { get; init; }
//    public string TagName { get; init; }

//}

