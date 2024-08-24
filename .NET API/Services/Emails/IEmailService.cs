using FoodDelivery.Models.DominModels.Auth;
using FoodDelivery.Models.DTO.AuthDTO;
using FoodDelivery.Services.Common;

namespace FoodDelivery.Services.Emails;

public interface IEmailService
{
    Task<SingleResult<bool>> SendEmailConfirmation(EmailData email);
}
