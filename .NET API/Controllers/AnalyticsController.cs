using FoodDelivery.Enums;
using FoodDelivery.Models.DTO.MealDTO;
using FoodDelivery.Services.Meals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
//[Authorize(Roles = "Chief")]
//[Authorize(Roles = "Admin")]
[Authorize]
[ApiController]
public class AnalyticsController (IMealService analytics) : ControllerBase
{
    private readonly IMealService _analytics = analytics;

    [ProducesResponseType(typeof(List<GetMealAnalysis>), 200)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [HttpGet("GetMealAnalytics")]
    public async Task<IActionResult> GetMealAnalytics()
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var UserID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        var result = await _analytics.GetMealAnalyses(UserID.ToString());

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }
    }
    [ProducesResponseType(typeof(List<GetIngredientAnalysis>), 200)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [HttpGet("GetIngredientAnalytics")]
    public async Task<IActionResult> GetIngredientAnalytics()
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        var result = await _analytics.GetIngredientAnalysis(ChiefID.ToString());

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }
    }
    [ProducesResponseType(typeof(List<GetCustomerAnalsis>), 200)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [HttpGet("GetCustomerAnalytics")]
    public async Task<IActionResult> GetCustomerAnalytics()
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var UserID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        var result = await _analytics.GetCustomerAnalsis(UserID.ToString());

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }
    }

    [ProducesResponseType(typeof(List<GetChiefAnalyticsRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [Authorize(Roles = "Admin")]
    [HttpGet("GetChiefAnalytics")]
    public async Task<IActionResult> GetChiefAnalytics()
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var UserID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        var result = await _analytics.GetChiefAnalytics();

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }
    }

    [ProducesResponseType(typeof(List<GetChartData>), 200)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [HttpGet("GetMealChartData")]
    public async Task<IActionResult> GetMealChartData(Guid MealOptionID)
    {

        var result = await _analytics.GetMealChartData(MealOptionID);

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }
    }

    [ProducesResponseType(typeof(List<GetChartData>), 200)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [HttpGet("GetCustomerChartData")]
    public async Task<IActionResult> GetCustomerChartData(Guid CustomerID)
    {

        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var UserID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        bool IsAdmin = HttpContext.User.Claims.Any(x => x.Value == "Admin" && x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

        var result = await _analytics.GetCustomerChartData(CustomerID.ToString(), IsAdmin, UserID.ToString());

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }
    }

    [ProducesResponseType(typeof(List<GetChartData>), 200)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [Authorize(Roles = "Admin")]
    [HttpGet("GetChiefChartData")]
    public async Task<IActionResult> GetChiefChartData(Guid ChiefID)
    {

        var result = await _analytics.GetChiefChartData(ChiefID.ToString());

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }
    }

    [ProducesResponseType(typeof(List<GetChartData>), 200)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [Authorize(Roles = "Chief")]
    [HttpGet("GetIngredientChartData")]
    public async Task<IActionResult> GetIngredientChartData(FoodIngredient ingredient)
    {

        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        var result = await _analytics.GetIngredientChartData(ingredient, ChiefID.ToString());

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }
    }
}
