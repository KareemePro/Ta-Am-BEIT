using FoodDelivery.Models.DominModels.Address;
using FoodDelivery.Models.DTO.AddressDTO;
using FoodDelivery.Services.Addresses;
using FoodDelivery.Services.Meals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[Route("[controller]")]
[ApiController]
public class AddressController(IAddressService address) : ControllerBase
{
    private readonly IAddressService _address = address;

    [HttpGet("Governorate")]
    [ProducesResponseType(typeof(List<Governorate>), 200)]
    public async Task<IActionResult> Governorate()
    {
        return Ok(await _address.GetGovernorates());
    }

    [HttpGet("District")]
    [ProducesResponseType(typeof(List<GetDistrictRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> District(Guid ID)
    {
        var result = await _address.GetDistricts(ID);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
    }

    [HttpGet("Street")]
    [ProducesResponseType(typeof(List<GetStreetRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> Street(Guid ID)
    {
        var result = await _address.GetStreets(ID);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
    }

    [HttpGet("Building")]
    [ProducesResponseType(typeof(List<GetBuildingRequest>), 200)]
    [ProducesResponseType(typeof(List<string>), 404)]
    public async Task<IActionResult> Building(Guid ID)
    {
        var result = await _address.GetBuildings(ID);
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