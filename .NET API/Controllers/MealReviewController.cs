using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.MealDTO;
using FoodDelivery.Models.DTO.MealReviewDTO;
using FoodDelivery.Services.MealReviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]
public class MealReviewController : ControllerBase
{
    private readonly IMealReviewService _reviews;
    public MealReviewController(IMealReviewService reviews)
    {
        _reviews = reviews;   
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Models.DTO.MealReviewDTO.GetMealReviewRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> GetCustomerReview(string id)
    {
                
        var result = await _reviews.GetReviewByCustomer(id);

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

    [HttpGet("GetMealReview")]
    [ProducesResponseType(typeof(GetMealReviewsRequest), 200)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> GetMealReviews(Guid MealID)
    {
        var result = await _reviews.GetMealReviews(MealID);

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

    [HttpPost]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(void), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]

    public async Task<IActionResult> UpsertMealReview([FromForm] UpsertMealReviewRequest request)
    {
        var CustomerID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

        if (!ModelState.IsValid || CustomerID == Guid.Empty) return BadRequest(ModelState);

        var result = await _reviews.UpsertMealReview(request, CustomerID.ToString());

        if (result)
        {
            return Ok();
        }
        return NotFound("this customer or meal id doesn't exist");
    }

    //[HttpPut]
    //[ProducesResponseType(typeof(void), 204)]
    //[ProducesResponseType(typeof(List<string>), 400)]
    //[ProducesResponseType(typeof(List<string>), 404)]

    //public async Task<IActionResult> EditMealReview([FromForm] UpsertMealReviewRequest request)
    //{
    //    if (!ModelState.IsValid) return BadRequest(ModelState);

    //    var result = await _reviews.EditMealReview(request);

    //    if (result) return NoContent();
    //    return NotFound("this customer or meal id doesn't exist");
    //}

    [HttpDelete]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> DeleteMealReview(Guid mealId, string customerId)
    {
        var result = await _reviews.DeleteMealReview(mealId, customerId);

        if (result) return NoContent();
        return NotFound("this customer or meal id doesn't exist");
    }
}
