using Azure.Core;
using FoodDelivery.Models.DTO.SubscriptionDTO;
using FoodDelivery.Services.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using X.Paymob.CashIn.Models.Callback;
using X.Paymob.CashIn;
using System.Text.Json.Serialization;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionServices _subscription;
    public SubscriptionController(ISubscriptionServices subscription)
    {
        _subscription = subscription;
    }
    [HttpGet("{SubscriptionID:guid}")]
    [ProducesResponseType(typeof(GetSubscriptionRequest), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> GetSubscription(Guid SubscriptionID)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _subscription.GetSubscription(SubscriptionID);

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int)result.HttpStatusCode
        };
    }


    [HttpPost]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _subscription.AddSubscription(request);

        if (result.IsSuccess)
        {
            return Created(result.Data.ToString(), null);
        }

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int)result.HttpStatusCode
        };
    }

    [HttpPut]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> Updatesubscription(UpdateSubscriptionStatusRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _subscription.EditSubscriptionStatus(request);

        if (result)
        {
            return NoContent();
        }
        return NotFound();

    }
}
