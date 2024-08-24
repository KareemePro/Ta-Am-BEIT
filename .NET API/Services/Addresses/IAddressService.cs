using FoodDelivery.Models.DominModels.Address;
using FoodDelivery.Models.DTO.AddressDTO;
using FoodDelivery.Services.Common;

namespace FoodDelivery.Services.Addresses;

public interface IAddressService
{
    Task<IEnumerable<Governorate>> GetGovernorates();

    Task<ListResult<GetDistrictRequest>> GetDistricts(Guid GovernorateID);

    Task<ListResult<GetStreetRequest>> GetStreets(Guid DistrictID);

    Task<ListResult<GetBuildingRequest>> GetBuildings(Guid StreetID);

    Task<GetFullAddressRequest> GetFullAddress(Guid BuildingID);
}
