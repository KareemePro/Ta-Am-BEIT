using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.MealDTO;
using FoodDelivery.Models.Utility;
using FoodDelivery.Services.Common;
using System.Threading.Tasks;

namespace FoodDelivery.Services.Meals;

public interface IMealService
{
    Task<IEnumerable<GetMealRequest>> GetMeals(FilteringData filteringData);

    Task<ListResult<GetCartMealRequest>> GetCartMeals(List<Guid> MealOptionIDs);

    Task<IEnumerable<GetMealTableRequest>> GetMealTable(FilteringData filteringData);

    Task<ListResult<GetMealAnalysis>> GetMealAnalyses(string UserID);

    Task<ListResult<GetCustomerAnalsis>> GetCustomerAnalsis(string UserID);

    Task<ListResult<GetChiefAnalyticsRequest>> GetChiefAnalytics();

    Task<ListResult<GetIngredientAnalysis>> GetIngredientAnalysis(string ChiefID);

    Task<SingleResult<GetMealRequest>> GetMeal(Guid ID);

    Task<ListResult<GetMealRequest>> GetSimilarMeals(Guid MealID);

    Task<SingleResult<object>> UpdateMeal(UpdateMealRequest updateMealRequest);

    Task<SingleResult<bool>> DisableMeal(Guid ID, Guid ChiefID);

    Task<bool> DeleteMeal(Guid ID);

    Task<ListResult<Guid>> AddMealOption(CreateMealOptionRequest request, byte[] image, Guid ChiefID);

    Task<bool> UpdateMealOption(UpdateMealOptionRequest request, byte[] image, Guid ChiefID);

    Task<ListResult<GetChartData>> GetMealChartData(Guid MealOptionID);

    Task<ListResult<GetChartData>> GetCustomerChartData(string CustomerID, bool IsAdmin, string? ChiefID);

    Task<ListResult<GetChartData>> GetIngredientChartData(FoodIngredient Ingredient, string ChiefID);

    Task<ListResult<GetChartData>> GetChiefChartData(string ChiefID);

    Task<bool> DeleteMealOption();
}
