using FoodDelivery.Models.DominModels.Subscriptions;
using FoodDelivery.Models.DTO.PromoCodeDTO;
using FoodDelivery.Models.DTO.SubscriptionDTO;
using FoodDelivery.Models.Utility;
using FoodDelivery.Services.PromoCodeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Security;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]
public class PromoCodeController : ControllerBase
{
    readonly private IPromoCodeService _promo;

    public PromoCodeController(IPromoCodeService promo)
    {
        _promo = promo;
    }

    [HttpPost("CreatePromoCode")]
    [ProducesResponseType(typeof(GetSubscriptionRequest), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> CreatePromoCode(UpsertPromoCodeRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _promo.AddPromoCode(request);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int)result.HttpStatusCode
        };
    }
    [HttpPut]
    [ProducesResponseType(typeof(GetSubscriptionRequest), 204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> UpdatePromoCode(UpsertPromoCodeRequest request)
    {
        var validationErrors = new List<string>();

        if (!ModelState.IsValid)
        {
            validationErrors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }
        if (request.ExpireDate < DateTime.Now)
        {
            validationErrors.Add("You can not have an expiration date in the past!");
        }
        if (validationErrors.Count != 0)
        {
            foreach (var error in validationErrors)
            {
                ModelState.AddModelError("ValidationErrors", error);
            }
            return BadRequest(ModelState);
        }

        var result = await _promo.AddPromoCode(request);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int)result.HttpStatusCode
        };
    }
    [HttpPost("{Hello:int}")]
    [ProducesResponseType(typeof(GetSubscriptionRequest), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> CreateCustomerPromoCode([FromBody]CreateCustomersPromoCodes request)
    {

        var validationErrors = new List<string>();

        if (!ModelState.IsValid)
        {
            validationErrors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }

        if (request.CustomersID.Distinct().Count() != request.CustomersID.Count)
        {
            validationErrors.Add("Duplicate entries in customer id");
        }

        if (validationErrors.Count != 0)
        {
            foreach (var error in validationErrors)
            {
                ModelState.AddModelError("ValidationErrors", error);
            }
            return BadRequest(ModelState);
        }


        var result = await _promo.AddCustomersPromoCodes(request);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int)result.HttpStatusCode
        };
    }
    [HttpPost("DiscountCalculate")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(float), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> MaxDiscount([FromBody] string PromoCodeID)
    {
        var CustomerID = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value;

        //if (request.MealData.Select(x => x.MealOptionID).Distinct().Count() != request.MealData.Count)
        //{
        //    ModelState.AddModelError("Duplicate entry", "please refresh your cart");
        //    return BadRequest(ModelState);
        //}
        //request.CustomerID = CustomerID;

        var result = await _promo.CalculateDiscount(CustomerID, PromoCodeID);

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int)result.HttpStatusCode
        };
    }
}
