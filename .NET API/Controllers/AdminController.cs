using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.AdminDTO;
using FoodDelivery.Models.DTO.CheifDTO;
using FoodDelivery.Models.DTO.MealDTO;
using FoodDelivery.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[Authorize(Roles = "Admin")]
[ApiController]
public class AdminController(IAdminService admin) : ControllerBase
{
    private readonly IAdminService _admin = admin;

    [ProducesResponseType(typeof(List<GetWaitingUserList>), 200)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [HttpGet("GetWaitingUserList")]
    public async Task<IActionResult> GetWaitingUserList()
    {
        var result = await _admin.GetWaitingUserList();

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

    [ProducesResponseType(typeof(GetChiefProfileDataRequest), 200)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [HttpGet("GetWaitingUser")]
    public async Task<IActionResult> GetWaitingUser(string ChiefID)
    {
        var result = await _admin.GetWaitingUser(ChiefID);

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

    [ProducesResponseType(202)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [HttpPatch("UpdateUserStatus")]
    public async Task<IActionResult> UpdateUserStatus(string UserID,bool IsEnabled)
    {
        if (IsEnabled)
        {
            var result = await _admin.EnableUser(UserID);

            if (result.IsSuccess)
            {
                return Accepted();
            }
            else
            {
                return new ObjectResult(result.Errors)
                {
                    StatusCode = (int)result.HttpStatusCode
                };
            }
        }
        else
        {
            var result = await _admin.DesableUser(UserID);

            if (result.IsSuccess)
            {
                return Accepted();
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
}
