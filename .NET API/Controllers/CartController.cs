using Azure.Core;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.CartDTO;
using FoodDelivery.Services.CartService;
using FoodDelivery.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cart;

    public CartController(ICartService cart)
    {
        _cart = cart; 
    }

    //[HttpPost]
    //[Authorize]
    //[ProducesResponseType(typeof(void),201)]
    //[ProducesResponseType(typeof(List<string>),404)]
    //[ProducesResponseType(typeof(List<string>),409)]
    //[ProducesResponseType(typeof(List<string>),400)]
    //public async Task<IActionResult> UpsertCart(List<UpsertCartItemRequest> request)
    //{
    //    var UserID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

    //    if (!ModelState.IsValid || UserID == Guid.Empty) return BadRequest(ModelState);

    //    var result = await _cart.UpsertCart(UserID.ToString());

    //    if (result.IsSuccess) return Created();

    //    return new ObjectResult(result.Errors)
    //    {
    //        StatusCode = (int?)result.HttpStatusCode
    //    };
    //}
    [HttpGet]
    [ProducesResponseType(typeof(GetCartRequest), 200)]
    [ProducesResponseType(typeof(string[]), 404)]
    [ProducesResponseType(typeof(string[]), 400)]
    public  async Task<IActionResult> GetCart()
    {
        var UserID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

        if (!ModelState.IsValid || UserID == Guid.Empty) return BadRequest(ModelState);

        var result = await _cart.RefreshCart(UserID, null,null);

        if (result.IsSuccess) return Ok(result.Data);

        return Accepted(result);
    }


    [HttpPost]
    [ProducesResponseType(typeof(GetCartRequest), 200)]
    [ProducesResponseType(typeof(CartResult<GetCartRequest>), 202)]
    public async Task<IActionResult> RefreshCart(List<UpsertCartItemRequest>? request, TimeOnly? TimeOfDelivery)
    {
        var UserID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

        if (request != null)
        {
            request = request
                .Select(x => new
                {
                    x.MealOptionID,
                    SideDishes = x.SideDishes == null ? null : x.SideDishes
                        .Select(z => new { z.MealSideDishID, z.MealSideDishOptionID, z.SideDishSizeOption })
                        .OrderBy(z => z.MealSideDishID) // Ensure consistent order
                        .ToList(),
                    x.Quantity
                })
                .Where(x => x.SideDishes != null) // Exclude items with null SideDishes
                .GroupBy(x => new
                {
                    x.MealOptionID,
                    SideDishesHash = x.SideDishes == null ? null : string.Join(",", x.SideDishes.Select(z => $"{z.MealSideDishID}-{z.MealSideDishOptionID}-{z.SideDishSizeOption}")),
                    x.Quantity
                })
                .Where(x => x.Key.SideDishesHash != null) // Exclude items with null SideDishesHash
                .Select(g => new UpsertCartItemRequest()
                {
                    MealOptionID = g.Key.MealOptionID,
                    Quantity = g.Key.Quantity,
                    SideDishes = g.First().SideDishes.Select(sideDish => new Models.DTO.CartDTO.SelectedSideDish(){
                        MealSideDishID = sideDish.MealSideDishID,
                        MealSideDishOptionID = sideDish.MealSideDishOptionID,
                        SideDishSizeOption = sideDish.SideDishSizeOption
                    }).ToList()
                })
                .ToList();


        }

        if (!ModelState.IsValid || UserID == Guid.Empty) return BadRequest(ModelState);

        var result = await _cart.RefreshCart(UserID, request, TimeOfDelivery);

        if (result.IsSuccess) return Ok(result.Data);

        return Accepted(result);
    }

    [HttpPatch]
    [ProducesResponseType(typeof(GetCartRequest), 200)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> UpdateCartItem(int cartItemID, int amount)
    {
        var UserID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

        if (!ModelState.IsValid || UserID == Guid.Empty) return BadRequest(ModelState);

        var result = await _cart.ChangeCartItemQTY(amount, cartItemID, UserID.ToString());
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

    [HttpDelete]
    [ProducesResponseType(typeof(void), 202)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> DeleteCart(DeleteCartItemRequest request)
    {
        var UserID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

        if (!ModelState.IsValid || UserID == Guid.Empty) return BadRequest(ModelState);

        return await _cart.DeleteCartItem(request, UserID.ToString()) ? Accepted() : NotFound("This cart do not exist");     
    }
}
