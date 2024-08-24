using FoodDelivery.Models.DominModels.Subscriptions;
using FoodDelivery.Models.DTO.SubscriptionDTO;
using FoodDelivery.Services.Common;

namespace FoodDelivery.Services.Subscriptions
{
    public interface ISubscriptionServices
    {
        Task<SingleResult<Guid>> AddSubscription(CreateSubscriptionRequest request);

        Task<bool> EditSubscriptionStatus(UpdateSubscriptionStatusRequest request);

        Task<SingleResult<bool>> AddSubscriptionDayData(CreateSubscriptionDayDataRequest request);

        Task<SingleResult<GetSubscriptionRequest>> GetSubscription(Guid subscriptionID); 

        Task<SingleResult<bool>> EditSubscriptionMealOptionQuantity(UpdateSubscriptionMealOptionQuantityRequest request);

        Task<SingleResult<bool>> EditSubscriptionDayData(UpdateSubscriptionDayDataRequest request);

        Task<SingleResult<bool>> DeleteSubscriptionDayData(DeleteSubscriptionDayDataRequest request);
    }
}
