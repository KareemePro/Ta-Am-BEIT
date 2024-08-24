using FoodDelivery.Models.DTO.SideDishDTO;
using FoodDelivery.Services.Common;

namespace FoodDelivery.Services.SideDishes;

public interface ISideDishService
{
    Task<SingleResult<Guid>> AddSideDish(CreateSideDishRequest request, Guid ChiefID, byte[] image);
    Task<SingleResult<bool>> UpdateSideDish(UpdateSideDishRequest request, Guid ChiefID, byte[] image);
    Task<SingleResult<bool>> DisableSideDish(Guid SideDishID, Guid ChiefID);
    Task<SingleResult<bool>> AddSideDishOption(CreateSideDishOptionRequest request, Guid ChiefID);
    Task<SingleResult<bool>> UpdateSideOptionDish(UpdateSideDishOptionRequest request, Guid ChiefID);
    Task<SingleResult<GetSideDishRequest>> GetSideDish(Guid SideDishID);
    Task<ListResult<GetSideDishRequest>> GetChiefSideDishes(Guid ChiefID);
    Task<ListResult<GetSideDishOptionRequest>> GetChiefSideDishOptions(Guid ChiefID);
}
