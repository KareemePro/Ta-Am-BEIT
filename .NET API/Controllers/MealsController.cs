using FoodDelivery.Models.DTO.MealDTO;
using FoodDelivery.Services.Meals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using FoodDelivery.Services.Common;
using FoodDelivery.Models.Utility;
using System.Text.Json;
using FoodDelivery.Models.DominModels;
using Microsoft.AspNetCore.Http.HttpResults;
using FoodDelivery.Models.DTO.AddressDTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OutputCaching;
using Azure.Core;
using Newtonsoft.Json.Linq;
using System;
using FoodDelivery.Enums;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]
public class MealsController(IMealService meal) : ControllerBase
{
    private readonly IMealService _meal = meal;

    [HttpGet]
    [ProducesResponseType(typeof(List<GetMealRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Meals([FromQuery] FilteringData filteringData)
    {
        return Ok(await _meal.GetMeals(filteringData));
    }

    [Authorize(Roles = "Chief")]
    [HttpGet("ChiefMeals")]
    [ProducesResponseType(typeof(List<GetMealTableRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> ChiefMeals([FromQuery] FilteringData filteringData)
    {
        var ChiefID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

        if (!ModelState.IsValid || ChiefID == Guid.Empty) return BadRequest(ModelState);

        filteringData.ChiefFilter = [ChiefID];

        return Ok(await _meal.GetMealTable(filteringData));

    }
    [Authorize]
    [HttpPost("MealOptionCart")]
    [ProducesResponseType(typeof(List<GetCartMealRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> MealOptionCart(List<Guid> MealOptionIDs)
    {
        var UserID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

        if (MealOptionIDs.Distinct().Count() != MealOptionIDs.Count)
        {
            ModelState.AddModelError("Duplicate entry", "please refresh your cart");
            return BadRequest(ModelState);
        }

        var result = await _meal.GetCartMeals(MealOptionIDs);

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
    [HttpGet("{MealID:guid}")]
    [ProducesResponseType(typeof(GetMealRequest), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]

    public async Task<IActionResult> Meal([FromRoute] Guid MealID)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _meal.GetMeal(MealID);
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
    [Authorize(Roles = "Chief")]
    [HttpDelete]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> DeleteMeal(Guid MealID)
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (!ModelState.IsValid || ChiefID == Guid.Empty) return BadRequest(ModelState);

        var result = await _meal.DisableMeal(MealID, ChiefID);

        if (result.IsSuccess)
        {
            return Accepted(result.Data);
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }
    }

    [HttpGet("GetSimilarMeals")]
    [ProducesResponseType(typeof(List<GetMealRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> Meals(Guid MealID)
    {
        var result = await _meal.GetSimilarMeals(MealID);

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
