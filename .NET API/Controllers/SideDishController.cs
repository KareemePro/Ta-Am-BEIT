using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.MealDTO;
using FoodDelivery.Models.DTO.SideDishDTO;
using FoodDelivery.Services.MealReviews;
using FoodDelivery.Services.SideDishes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]
public class SideDishController(ISideDishService sideDish) : ControllerBase
{
    private readonly ISideDishService _sideDish = sideDish;

    [HttpPost("CreateSideDish")]
    [Authorize(Roles = "Chief")]
    [ProducesResponseType(typeof(Uri), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> CreateSideDish(CreateSideDishRequest request)
    {
        var ChiefID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

        if (!ModelState.IsValid || ChiefID == Guid.Empty) return BadRequest(ModelState);

        var base64 = request.Image;
        base64 = base64.Replace("data:image/jpeg;base64,", "");

        var bytes = Convert.FromBase64String(base64);

        var result = await _sideDish.AddSideDish(request, ChiefID, bytes);

        if (result.IsSuccess)
        {
            return Created(result.Data.ToString(), null);
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }

    }

    [HttpPut("UpdateSideDish")]
    [Authorize(Roles = "Chief")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> UpdateSideDish(UpdateSideDishRequest request)
    {
        var ChiefID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

        if (!ModelState.IsValid || ChiefID == Guid.Empty) return BadRequest(ModelState);

        byte[]? bytes = null;
        var base64 = request.Image;
        if (base64 != null)
        {
            base64 = base64.Replace("data:image/jpeg;base64,", "");
            bytes = Convert.FromBase64String(base64);
        }

        var result = await _sideDish.UpdateSideDish(request, ChiefID, bytes);

        if (result.IsSuccess)
        {
            return NoContent();
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }

    }

    [HttpGet("{SideDishID:guid}")]
    [ProducesResponseType(typeof(GetSideDishRequest), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> GetSideDish([FromRoute] Guid SideDishID)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _sideDish.GetSideDish(SideDishID);
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

    [HttpGet]
    [ProducesResponseType(typeof(List<GetSideDishRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> GetChiefSideDishes([FromRoute] Guid? ChiefID)
    {

        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefIDFromJWT = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (ModelState.IsValid && (ChiefID == Guid.Empty && ChiefIDFromJWT == Guid.Empty)) return BadRequest(ModelState);

        var result = await _sideDish.GetChiefSideDishes(ChiefID ?? ChiefIDFromJWT);
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
    public async Task<IActionResult> DeleteSideDish(Guid SideDishID)
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (!ModelState.IsValid || ChiefID == Guid.Empty) return BadRequest(ModelState);

        var result = await _sideDish.DisableSideDish(SideDishID, ChiefID);

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

}
