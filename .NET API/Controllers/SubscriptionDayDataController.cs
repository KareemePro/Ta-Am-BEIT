using FoodDelivery.Models.DominModels.Subscriptions;
using FoodDelivery.Models.DTO.SubscriptionDTO;
using FoodDelivery.Services.Addresses;
using FoodDelivery.Services.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]
public class SubscriptionDayDataController : ControllerBase
{
    private readonly ISubscriptionServices _subscription;

    public SubscriptionDayDataController(ISubscriptionServices subscription)
    {
        _subscription = subscription;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> CreateSubscriptionDayData(CreateSubscriptionDayDataRequest request)
    {
        var validationErrors = new List<string>();

        if (!ModelState.IsValid)
        {
            validationErrors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }

        if (request.CreateSubscriptionMealOptionRequests.DistinctBy(x => x.MealOptionID).Count() != request.CreateSubscriptionMealOptionRequests.Count)
        {
            validationErrors.Add("Duplicate entries in meal option");
        }

        if (request.DeliveryDate > DateTime.Now.AddDays(30))
        {
            validationErrors.Add("You cannot have a delivery time that is more than 30 days in the future");
        }

        if (request.DeliveryDate < DateTime.Now.AddDays(1))
        {
            validationErrors.Add("You cannot have a delivery time that is before one day from now");
        }

        if (validationErrors.Count != 0)
        {
            foreach (var error in validationErrors)
            {
                ModelState.AddModelError("ValidationErrors", error);
            }
            return BadRequest(ModelState);
        }

        var result = await _subscription.AddSubscriptionDayData(request);

        if (result.IsSuccess)
        {
            return Created(result.Data.ToString(), null);
        }

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int)result.HttpStatusCode
        };
    }

    [HttpPatch]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(List<string>), 404)]
    [ProducesResponseType(typeof(List<string>), 409)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> UpdateSubscriptionMealOptionQuantity(UpdateSubscriptionMealOptionQuantityRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _subscription.EditSubscriptionMealOptionQuantity(request);
        if (result.IsSuccess) return NoContent();

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int?)result.HttpStatusCode
        };
    }



    [HttpPut]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(List<string>), 409)]
    [ProducesResponseType(typeof(List<string>), 404)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> UpdateSubscriptionDayData(UpdateSubscriptionDayDataRequest request)
    {
        var validationErrors = new List<string>();

        if (!ModelState.IsValid)
        {
            validationErrors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }

        if (request.NewDeliveryDate > DateTime.Now.AddDays(30))
        {
            validationErrors.Add("You cannot have a delivery time that is more than 30 days in the future");
        }

        if (request.NewDeliveryDate < DateTime.Now.AddDays(1))
        {
            validationErrors.Add("You cannot have a delivery time that is before one day from now");
        }

        if (validationErrors.Count != 0)
        {
            foreach (var error in validationErrors)
            {
                ModelState.AddModelError("ValidationErrors", error);
            }
            return BadRequest(ModelState);
        }

        var result = await _subscription.EditSubscriptionDayData(request);

        if (result.IsSuccess) return NoContent();

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int?)result.HttpStatusCode
        };

    }

    [HttpDelete]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(List<string>), 409)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> DeleteSubscriptionDayData(DeleteSubscriptionDayDataRequest request)
    {
        var result = await _subscription.DeleteSubscriptionDayData(request);

        if (result.IsSuccess) return NoContent();

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int?)result.HttpStatusCode
        };

    }


}
