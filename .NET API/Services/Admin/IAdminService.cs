using FoodDelivery.Models.DTO.AdminDTO;
using FoodDelivery.Models.DTO.CheifDTO;
using FoodDelivery.Services.Common;

namespace FoodDelivery.Services.Admin;

public interface IAdminService
{
    Task<ListResult<GetWaitingUserList>> GetWaitingUserList();
    Task<SingleResult<GetChiefProfileDataRequest>> GetWaitingUser(string ChiefID);
    Task<SingleResult<bool>> EnableUser(string UserID);
    Task<SingleResult<bool>> DesableUser(string UserID);
}
