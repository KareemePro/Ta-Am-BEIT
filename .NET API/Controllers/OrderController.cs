using FoodDelivery.Models.DTO.OrderDTO;
using FoodDelivery.Services.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using X.Paymob.CashIn.Models.Callback;
using X.Paymob.CashIn;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using X.Paymob.CashIn.Models.Payment;
using FoodDelivery.Services.Emails;
using Azure.Core;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]
public class OrderController(IOrderService order, IEmailService email) : ControllerBase
{
    private readonly IOrderService _order = order;

    private readonly IEmailService _email = email;

    [HttpPost]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(CashInPaymentKeyResponse), 200)]
    [ProducesResponseType(typeof(List<string>), 409)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> AddOrder(CreateOrderRequest request)
    {
        var CusteomerID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

        if (!ModelState.IsValid || CusteomerID == Guid.Empty) return BadRequest(ModelState);

        var result = await _order.CreateOrder(request, CusteomerID);
        if (result.IsSuccess) return Ok(result.Data);

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int?)result.HttpStatusCode
        };
    }

    [HttpGet("GetCustomerOrders")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(List<GetOrderRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> GetCustomerOrder()
    {
        var CusteomerID = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid").Value);

        if (!ModelState.IsValid || CusteomerID == Guid.Empty) return BadRequest(ModelState);

        var result = await _order.GetOrderDetails(CusteomerID.ToString());
        if (result.IsSuccess) return Ok(result.Data);

        return new ObjectResult(result.Errors)
        {
            StatusCode = (int?)result.HttpStatusCode
        };
    }


    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    [HttpPost("cashin-callback")]
    public ActionResult CashInCallback(
        [FromQuery] string hmac,
        [FromBody] CashInCallback callback,
        IPaymobCashInBroker broker
    )
    {
        //var h = new Models.DTO.AuthDTO.EmailData()
        //{
        //    To = "omar.1met@gmail.com",
        //    Subject = "hello",
        //    Body = HttpContext.Connection.RemoteIpAddress.ToString(),
        //};
        //_email.SendEmailConfirmation(h);

        if (callback.Type is null || callback.Obj is null)
        {
            throw new InvalidOperationException("Unexpected transaction callback. first");
        }

        var content = ((JsonElement)callback.Obj).GetRawText();

        switch (callback.Type.ToUpperInvariant())
        {
            case CashInCallbackTypes.Transaction:
                {
                    var transaction = JsonSerializer.Deserialize<CashInCallbackTransaction>(content, SerializerOptions)!;
                    var valid = broker.Validate(transaction, hmac);

                    if (!valid)
                    {
                        return BadRequest("seoncd");
                    }

                    // TODO: Handle transaction.
                    //var a = new Models.DTO.AuthDTO.EmailData()
                    //{
                    //    To = "omar.1met@gmail.com",
                    //    Subject = "hello",
                    //    Body = "pay call back done two",
                    //};
                    //_email.SendEmailConfirmation(h);
                    return Ok();
                }
            case CashInCallbackTypes.Token:
                {
                    var token = JsonSerializer.Deserialize<CashInCallbackToken>(content, SerializerOptions)!;
                    var valid = broker.Validate(token, hmac);

                    if (!valid)
                    {
                        return BadRequest("third");
                    }

                    // TODO: Handle token.
                    //var n = new Models.DTO.AuthDTO.EmailData()
                    //{
                    //    To = "omar.1met@gmail.com",
                    //    Subject = "hello",
                    //    Body = "pay call back done three",
                    //};
                    //_email.SendEmailConfirmation(h);
                    return Ok();
                }
            default:
                throw new InvalidOperationException($"Unexpected {nameof(CashInCallbackTypes)} = {callback.Type} forth");
        }
    }
}
