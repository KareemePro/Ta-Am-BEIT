using FoodDelivery.Enums;
using FoodDelivery.Models.DominModels.Orders;
using FoodDelivery.Models.DTO.OrderDTO;
using FoodDelivery.Services.Common;
using X.Paymob.CashIn.Models.Payment;

namespace FoodDelivery.Services.Orders;

public interface IOrderService
{
    Task<SingleResult<CashInPaymentKeyResponse>> CreateOrder(CreateOrderRequest request, Guid CustomerID);

    Task<ListResult<GetOrderRequest>> GetOrderDetails(string CustomerID);

    Task<ListResult<GetOrderSummaryRequest>> GetOrdersSummary(string CustomerID);

    Task<ListResult<GetOrderItem>> GetChiefOrders(string ChiefID);

    Task<SingleResult<bool>> UpdateOrderItemStatus(int OrderItemID, string ChiefID, OrderStatus OrderItemUpdate);
}
