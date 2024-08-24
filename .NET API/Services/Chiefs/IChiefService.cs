using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.CheifDTO;
using FoodDelivery.Models.DTO.IngredientDTO;
using FoodDelivery.Services.Common;

namespace FoodDelivery.Services.Cheifs;

public interface IChiefService
{
    Task<SingleResult<bool>> UpsertChiefData(UpsertChiefDataRequest request, Guid ChiefID, byte[]? ChiefImage, byte[]? CoverImage, byte[]? HealthCertImage);
    Task<SingleResult<GetChiefProfileDataRequest>> GetChiefProfileData(Guid ChiefID);
    Task<SingleResult<bool>> AddIngredients(List<UpsertIngredientRequest> ingredients, Guid ChiefID);
    Task<SingleResult<bool>> RemoveIngredient(FoodIngredient ingredient, Guid ChiefID);
    Task<ListResult<GetChiefIngredientRequest>> GetIngredients(Guid ChiefID);
    Task<SingleResult<GetChiefMealsRequest>> GetChiefMeals(string ChiefID);

}
