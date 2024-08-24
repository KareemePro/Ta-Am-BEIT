using FoodDelivery.Models.DTO.PaymentDTO;
using FoodDelivery.Services.Common;
using System.Net;
using X.Paymob.CashIn.Models.Payment;

namespace FoodDelivery.Services.Payment;

public interface IPaymentService
{
    Task<SingleResult<bool>> SubscriptionPaymentProcess(PaySubscriptionDTO paySubscriptionDTO);
    Task<CashInPaymentKeyResponse> OrderPaymentProcess(PaymentDTO paymentDTO);
}
