using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DominModels.Auth;
using FoodDelivery.Models.DTO.AuthDTO;
using FoodDelivery.Models.DTO.CustomerDTO;
using FoodDelivery.Services.Common;

namespace FoodDelivery.Services.Auth
{
    public interface IAuthService
    {
        //Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<SingleResult<AuthModel>> CustomerSignUp(CustomerSignUpModel model);
        Task<SingleResult<AuthModel>> ChiefSignUp(ChiefSignUpModel model);
        Task<SingleResult<AuthModel>> GetTokenAsync(TokenRequestModel model); 
        Task<SingleResult<Customer>> GetCustomerByID(string customerID);
        Task<SingleResult<string>> AddRoleAsync(AddRoleModel model);
        Task<SingleResult<AuthModel>> RefreshTokenAsync(string token);
        Task<SingleResult<bool>> PostingCustomerProfileData(PostCustomerProfileDataRequest request, byte[]? image, string CustomerID);
        Task<SingleResult<GetCustomerProfileDataRequest>> GetCustomerProfileData(string CustomerID);
        Task<bool> RevokeTokenAsync(string token);
        Task<bool> EmailCheck(string email);
        Task<bool> EmailConfirmation(User user);
        Task<bool> EmailConfirmation(string email);
        Task<bool> EmailConfirmed(Guid user, string token);
        Task<SingleResult<bool>> SendResetPasswordEmail(string email);
        Task<SingleResult<bool>> ResetPassword(ResetPasswordRequest request);
    }
}