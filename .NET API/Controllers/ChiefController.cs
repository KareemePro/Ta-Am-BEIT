using Azure.Core;
using FoodDelivery.Enums;
using FoodDelivery.Models.DTO.CheifDTO;
using FoodDelivery.Models.DTO.IngredientDTO;
using FoodDelivery.Models.DTO.OrderDTO;
using FoodDelivery.Services.Cheifs;
using FoodDelivery.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]
public class ChiefController(IChiefService chief, IOrderService order) : ControllerBase
{
    private readonly IChiefService _chief = chief;

    private readonly IOrderService _order = order;

    [Authorize(Roles = "Chief")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> UpsertChiefData([FromBody] UpsertChiefDataRequest request)
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefIDFromJWT = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (ChiefIDFromJWT == Guid.Empty) return BadRequest(ModelState);


        byte[]? ChiefImage = null;
        var base64 = request.ChiefImage;
        if (base64 != null)
        {
            base64 = base64.Replace("data:image/jpeg;base64,", "");
            ChiefImage = Convert.FromBase64String(base64);
        }
        base64 = null;

        byte[]? CoverImage = null;
        base64 = request.CoverImage;
        if (base64 != null)
        {
            base64 = base64.Replace("data:image/jpeg;base64,", "");
            CoverImage = Convert.FromBase64String(base64);
        }
        base64 = null;

        byte[]? HealthCertImage = null;
        base64 = request.HealthCertImage;
        if (base64 != null)
        {
            base64 = base64.Replace("data:image/jpeg;base64,", "");
            HealthCertImage = Convert.FromBase64String(base64);
        }


        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _chief.UpsertChiefData(request, ChiefIDFromJWT, ChiefImage, CoverImage, HealthCertImage);
        if (result.IsSuccess)
        {
            return Ok();
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
    [HttpGet("GetChiefProfileData")]
    [ProducesResponseType(typeof(GetChiefProfileDataRequest), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> GetChiefProfileData()
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefIDFromJWT = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (ChiefIDFromJWT == Guid.Empty) return BadRequest(ModelState);

        var result = await _chief.GetChiefProfileData(ChiefIDFromJWT);

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
    [HttpPost("AddIngredients")]
    [ProducesResponseType(typeof(List<string>), 404)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> AddIngredients(List<UpsertIngredientRequest> request)
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefIDFromJWT = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (ChiefIDFromJWT == Guid.Empty) return BadRequest(ModelState);

        var ingredients = request.Select(x => x.Ingredient);

        if (ingredients != null)
        {
            if (ingredients.Any(Ingredient => !Enum.IsDefined(typeof(FoodIngredient), Ingredient) || ingredients.Count(t => t.Equals(Ingredient)) > 1))
            {
                return BadRequest(ModelState);
            }
        }

        var result = await _chief.AddIngredients(request, ChiefIDFromJWT);

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
    [HttpGet("GetIngredients")]
    [ProducesResponseType(typeof(List<string>), 404)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<GetChiefIngredientRequest>), 200)]
    public async Task<IActionResult> GetIngredients()
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefIDFromJWT = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (ChiefIDFromJWT == Guid.Empty) return BadRequest(ModelState);

        var result = await _chief.GetIngredients(ChiefIDFromJWT);

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
    [HttpDelete("RemoveIngredients")]
    [ProducesResponseType(typeof(List<string>), 404)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<bool>), 200)]
    public async Task<IActionResult> RemoveIngredients(FoodIngredient ingredient)
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefIDFromJWT = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (ChiefIDFromJWT == Guid.Empty) return BadRequest(ModelState);

        var result = await _chief.RemoveIngredient(ingredient, ChiefIDFromJWT);

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
    [HttpGet("GetChiefOrders")]
    [ProducesResponseType(typeof(List<GetOrderItem>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> GetChiefOrders()
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefIDFromJWT = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (ChiefIDFromJWT == Guid.Empty) return BadRequest(ModelState);

        var result = await _order.GetChiefOrders(ChiefIDFromJWT.ToString());

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
    [HttpPatch("UpdateOrderItemStatus")]
    [ProducesResponseType(202)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> UpdateOrderItemStatus(int OrderItemID, OrderStatus OrderItemUpdate)
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var ChiefIDFromJWT = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        if (ChiefIDFromJWT == Guid.Empty) return BadRequest(ModelState);

        var result = await _order.UpdateOrderItemStatus(OrderItemID, ChiefIDFromJWT.ToString(), OrderItemUpdate);

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

    [HttpGet("GetChiefMeals")]
    [ProducesResponseType(typeof(List<string>), 404)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(GetChiefMealsRequest), 200)]
    public async Task<IActionResult> GetIngredients(Guid ChiefID)
    {

        var result = await _chief.GetChiefMeals(ChiefID.ToString());

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
