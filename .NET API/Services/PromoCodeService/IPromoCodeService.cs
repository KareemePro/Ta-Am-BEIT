using FoodDelivery.Models.DominModels.Orders;
using FoodDelivery.Models.DominModels.Subscriptions;
using FoodDelivery.Models.DTO.PromoCodeDTO;
using FoodDelivery.Models.Utility;
using FoodDelivery.Services.Common;

namespace FoodDelivery.Services.PromoCodeService;

public interface IPromoCodeService
{
    Task<SingleResult<bool>> AddPromoCode(UpsertPromoCodeRequest request);
    Task<SingleResult<bool>> EditPromoCode(UpsertPromoCodeRequest request); 
    Task<DiscountData> GetPromoCodeDiscount(string promoCodeID, string CustomerID);
    Task<SingleResult<bool>> AddCustomersPromoCodes(CreateCustomersPromoCodes request);
    Task<SingleResult<Subscription>> AddPromoCodeToSubscription(Subscription subscription);
    Task<SingleResult<Order>> AddPromoCodeToOrder(Order order);
    Task<SingleResult<float>> CalculateDiscount(string CustomerID, string PromoCodeID);

}
