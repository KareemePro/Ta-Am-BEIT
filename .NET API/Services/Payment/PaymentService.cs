using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DTO.PaymentDTO;
using FoodDelivery.Services.Addresses;
using FoodDelivery.Services.Auth;
using FoodDelivery.Services.Common;
using Microsoft.AspNetCore.Mvc;
using X.Paymob.CashIn;
using X.Paymob.CashIn.Models.Orders;
using X.Paymob.CashIn.Models.Payment;

namespace FoodDelivery.Services.Payment;

public class PaymentService : IPaymentService
{
    private readonly IPaymobCashInBroker _broker;

    private readonly IAuthService _auth;

    private readonly IAddressService _address;


    public PaymentService(IPaymobCashInBroker broker, IAuthService auth, IAddressService address)
    {
        _broker = broker;
        _auth = auth;
        _address = address;
    }

    public async Task<CashInPaymentKeyResponse> OrderPaymentProcess(PaymentDTO paymentDTO)
    {
        var Address = await _address.GetFullAddress(paymentDTO.BuildingID);

        //change it
       // paymentDTO.TotalAmountInPennies = 50;

        var orderRequest = CashInCreateOrderRequest.CreateOrder(paymentDTO.TotalAmountInPennies,"EGP",paymentDTO.OrderID.ToString());

        var orderResponse = await _broker.CreateOrderAsync(orderRequest);
        var billingData = new CashInBillingData(
            firstName: paymentDTO.FirstName,
            lastName: paymentDTO.LastName,
            phoneNumber: paymentDTO.PhoneNumber,
            email: paymentDTO.Email,
            country: "Egypt",
            state: Address.GovernorateName,
            city: Address.DistrictName,
            street: Address.StreetName,
            building: Address.BuildingName,
            floor: paymentDTO.FloorNo,
            apartment: paymentDTO.ApartmentNo);

        var paymentKeyRequest = new CashInPaymentKeyRequest(
            integrationId: 4410226,
            orderId: orderResponse.Id,
            billingData: billingData,
            amountCents: paymentDTO.TotalAmountInPennies);

        var paymentKeyResponse = await _broker.RequestPaymentKeyAsync(paymentKeyRequest);
        return paymentKeyResponse;
    }

    public async Task<SingleResult<bool>> SubscriptionPaymentProcess(PaySubscriptionDTO paySubscriptionDTO)
    {
        var result = await _auth.GetCustomerByID(paySubscriptionDTO.CustomerID);
        if (!result.IsSuccess)
        {
            return SingleResult<bool>.Failure(result.Errors);
        }
        var customer = result.Data;
        var Address = await _address.GetFullAddress((Guid)customer.BuildingID);

        var orderRequest = CashInCreateOrderRequest.CreateOrder(paySubscriptionDTO.TotalAmountInPennies);

        var orderResponse = await _broker.CreateOrderAsync(orderRequest);
        var billingData = new CashInBillingData(
            firstName: customer.FirstName,
            lastName: customer.LastName,
            phoneNumber: customer.PhoneNumber,
            email: customer.Email,
            country: "Egypt",
            state: Address.GovernorateName,
            city: Address.DistrictName,
            street: Address.StreetName,
            building: Address.BuildingName,
            floor: "3");

        var paymentKeyRequest = new CashInPaymentKeyRequest(
            integrationId: 4410226,
            orderId: orderResponse.Id,
            billingData: billingData,
            amountCents: paySubscriptionDTO.TotalAmountInPennies);

        var paymentKeyResponse = await _broker.RequestPaymentKeyAsync(paymentKeyRequest);
        return SingleResult<bool>.Success(true);
    }
}
