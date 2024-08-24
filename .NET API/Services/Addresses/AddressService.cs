using FoodDelivery.Data;
using FoodDelivery.Models.DominModels.Address;
using FoodDelivery.Models.DTO.AddressDTO;
using FoodDelivery.Services.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FoodDelivery.Services.Addresses;

public class AddressService : IAddressService
{

    private readonly DBContext _context;

    public AddressService(DBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Governorate>> GetGovernorates()
    {
        return await _context.Governorates.ToArrayAsync();
    }

    public async Task<ListResult<GetDistrictRequest>> GetDistricts(Guid GovernorateID)
    {
        if(await _context.Districts.AnyAsync(x => x.GovernorateID == GovernorateID))
        {
            return ListResult<GetDistrictRequest>.Success(await _context.Districts.Where(x => x.GovernorateID == GovernorateID).Select(x => new GetDistrictRequest(x.ID, x.Name)).ToArrayAsync(), HttpStatusCode.OK);
        }
        return ListResult<GetDistrictRequest>.Failure([("This Governorate ID Doesn't Exist")], HttpStatusCode.NotFound);
    }

    public async Task<ListResult<GetStreetRequest>> GetStreets(Guid DistrictID)
    {
        if (await _context.Streets.AnyAsync(x => x.DistrictID == DistrictID))
        {
            return ListResult<GetStreetRequest>.Success(await _context.Streets.Where(x => x.DistrictID == DistrictID).Select(x => new GetStreetRequest(x.ID, x.Name)).ToArrayAsync(),HttpStatusCode.OK);
        }
        return ListResult<GetStreetRequest>.Failure([("This District ID Doesn't Exist")], HttpStatusCode.NotFound);
    }

    public async Task<ListResult<GetBuildingRequest>> GetBuildings(Guid StreetID)
    {
        if (await _context.Buildings.AnyAsync(x => x.StreetID == StreetID))
        {
            return ListResult<GetBuildingRequest>.Success(await _context.Buildings.Where(x => x.StreetID == StreetID).Select(x => new GetBuildingRequest(x.ID, x.Name)).ToArrayAsync(), HttpStatusCode.OK);
        }
        return ListResult<GetBuildingRequest>.Failure([("This Street ID Doesn't Exist")], HttpStatusCode.NotFound);
    }

    public async Task<GetFullAddressRequest> GetFullAddress(Guid BuildingID)
    {
        return await _context.Buildings
            .Where(x => x.ID == BuildingID)
            .Select(x => new GetFullAddressRequest(x.Street.District.Governorate.Name,
            x.Street.District.Name,
            x.Street.Name,
            x.Name,
            "3")).FirstAsync();
    }
}
