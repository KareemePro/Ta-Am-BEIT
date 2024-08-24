using FoodDelivery.Enums;
using FoodDelivery.Migrations;
using FoodDelivery.Models.DTO.MealDTO;
using FoodDelivery.Services.Meals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles ="Chief")]
public class MealOptionController : ControllerBase
{
    private readonly IMealService _meal;

    public MealOptionController(IMealService meal)
    {
        _meal = meal;
    }
    [HttpPost]
    [ProducesResponseType(typeof(Uri), 201)]
    [ProducesResponseType(typeof(List<string>), 404)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]

    public async Task<IActionResult> AddMealOption([FromBody] CreateMealOptionRequest request )
    {
        var base64 = request.Image;
        base64 = base64.Replace("data:image/jpeg;base64,","");

        var bytes = Convert.FromBase64String(base64);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (request.AddIngredients != null)
        {
            //if (request.AddIngredients.Any(Ingredient => !Enum.IsDefined(typeof(FoodIngredient), Ingredient) || request.AddIngredients.Count(t => t.Equals(Ingredient)) > 1))
            //{
            //    return BadRequest(ModelState);
            //}
        }



        //if (!ModelState.IsValid || request.AddIngredients == null || request.AddIngredients.Any(Ingredient => !Enum.IsDefined(typeof(FoodIngredient), Ingredient) || request.AddIngredients.Count(t => t.Equals(Ingredient)) > 1)) //|| request.mealOptionRequests.DistinctBy(x => x.SizeOptionID).Count() != request.mealOptionRequests.Count)
        //{
        //    //ModelState.AddModelError("Validation Errors", "Duplicate Entries");
        //    return BadRequest(ModelState);
        //}

        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        var result = await _meal.AddMealOption(request, bytes, ChiefID);
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
    [HttpPut]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(List<string>), 404)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> UpdateMealOption([FromBody] UpdateMealOptionRequest request)
    {
        byte[]? bytes = null;
        var base64 = request.Image;
        if (base64 != null)
        {
            base64 = base64.Replace("data:image/jpeg;base64,", "");
            bytes = Convert.FromBase64String(base64);
        }

        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;


        foreach (var ingredientString in request.AddIngredients)
        {
            if (!Enum.TryParse<FoodIngredient>(ingredientString.FoodIngredient.ToString(), out _))
            {
                return BadRequest($"Invalid ingredient: {ingredientString}");
            }
        }

        var result = await _meal.UpdateMealOption(request, bytes, ChiefID);
        if (result)
        {
            return NoContent();
        }
        else
        {
            return NotFound("This meal option doesn't exist");
        }
    }
}
