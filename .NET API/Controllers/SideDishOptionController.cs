using FoodDelivery.Models.DominModels.Meals;
using FoodDelivery.Models.DTO.SideDishDTO;
using FoodDelivery.Services.SideDishes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]
public class SideDishOptionController(ISideDishService sideDish) : ControllerBase
{
    private readonly ISideDishService _sideDish = sideDish;

    [HttpPost("CreateSideDishOption")]
    [Authorize(Roles = "Chief")]
    [ProducesResponseType(typeof(Uri), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> CreateSideDishOption(CreateSideDishOptionRequest request)
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (!ModelState.IsValid || ChiefID == Guid.Empty) return BadRequest(ModelState);

        var result = await _sideDish.AddSideDishOption(request, ChiefID);

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

    [HttpPut("UpdateSideDishOption")]
    [Authorize(Roles = "Chief")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> UpdateSideDishOption(UpdateSideDishOptionRequest request)
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (!ModelState.IsValid || ChiefID == Guid.Empty) return BadRequest(ModelState);

        var result = await _sideDish.UpdateSideOptionDish(request, ChiefID);

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

    [HttpGet("{ChiefID:guid?}")]
    [ProducesResponseType(typeof(List<GetSideDishOptionRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> GetChiefSideDishes([FromRoute] Guid? ChiefID)
    {

        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefIDFromJWT = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (ChiefID == null && ChiefIDFromJWT == Guid.Empty) return BadRequest(ModelState);

        var result = await _sideDish.GetChiefSideDishOptions(ChiefID ?? ChiefIDFromJWT);
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
