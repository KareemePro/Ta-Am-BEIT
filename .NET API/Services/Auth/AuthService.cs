using FoodDelivery.Models.DominModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Data;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Settings;
using FoodDelivery.Services.Common;
using System.Net;
using FoodDelivery.Models.DTO.ImageDTO;
using System.Linq;
using FoodDelivery.Services.Emails;
using Microsoft.AspNetCore.WebUtilities;
using FoodDelivery.Models.DTO.AuthDTO;
using System.Web;
using FoodDelivery.Models.DTO.CustomerDTO;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Services.PromoCodeService;

namespace FoodDelivery.Services.Auth;

public class AuthService(DBContext context, UserManager<User> userManager, UserManager<Chief> chiefManager, UserManager<Customer> customerManager, RoleManager<IdentityRole> roleManager,
    IOptions<JWT> jwt, IEmailService emailService, IImageService image, IConfiguration config, IPromoCodeService promo) : IAuthService
{
    private readonly DBContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly UserManager<Chief> _chiefManager = chiefManager;
    private readonly UserManager<Customer> _customerManager = customerManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly JWT _jwt = jwt.Value;
    //private readonly IImageService _imageService = imageService; 
    private readonly IEmailService _emailService = emailService;
    private readonly IImageService _image = image;
    private readonly IConfiguration _config = config;
    private readonly IPromoCodeService _promo = promo;



    /*public async Task<AuthModel> RegisterAsync(RegisterModel model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) is not null)
            return new AuthModel { Message = "Email is already registered!" };

        if (await _userManager.FindByNameAsync(model.Username) is not null)
            return new AuthModel { Message = "Username is already registered!" };

        var user = new User
        {
            UserName = model.Username,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserImage = model.Image,
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return new AuthModel { Message = errors };
        }

        await _userManager.AddToRoleAsync(user, "User");

        var jwtSecurityToken = await CreateJwtToken(user);

        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        await _userManager.UpdateAsync(user);

        return new AuthModel
        {
            Email = user.Email,
            ExpiresOn = jwtSecurityToken.ValidTo,
            IsAuthenticated = true,
            Roles = ["User"],
            jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Username = user.UserName,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = (DateTime)user.RefreshTokenExpiresOn,
        };
    }*/

    public async Task<SingleResult<AuthModel>> CustomerSignUp(CustomerSignUpModel model)
    {
        if (await _customerManager.FindByEmailAsync(model.Email) is not null)
            return SingleResult<AuthModel>.Failure([("Email is already registered!")], HttpStatusCode.Conflict);

        /*if (await _customerManager.FindByNameAsync(model.Username) is not null)
            return new AuthModel { Message = "Username is already registered!" };*/

        var user = new Customer
        {
            UserName = $"{model.FirstName}{model.LastName}",
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            IsEnabled = true,
            //LockoutEnabled = false,
            SignupDate = DateTime.Now,
        };

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var result = await _customerManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";
                return SingleResult<AuthModel>.Failure(result.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest);
            }

            await _customerManager.AddToRoleAsync(user, "Customer");

            var jwtSecurityToken = await CreateJwtToken(user);

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresOn = DateTime.UtcNow.AddDays(10);
            user.IsRefreshTokenRevoked = false;

            /*if (model.Image != null)
            {
                user.UserImage = await _imageService.Process(new ImageInput() { Content = model.Image.OpenReadStream(), FileName = Guid.NewGuid().ToString(), Path = "Images/UserImage" });
            }*/

            //add error message
            if(!await EmailConfirmation(user))
            {
            }

            await _customerManager.UpdateAsync(user);
            await transaction.CommitAsync();

            await _promo.AddCustomersPromoCodes(new Models.DTO.PromoCodeDTO.CreateCustomersPromoCodes() { CustomersID = [user.Id], PromoCodeID = "Get 10% Off" });
            return SingleResult<AuthModel>.Success(new AuthModel
            {
                ExpiresOn = jwtSecurityToken.ValidTo,
                Roles = new List<string> { "Customer" },
                jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken,
                RefreshTokenExpiration = (DateTime)user.RefreshTokenExpiresOn,
            }, HttpStatusCode.Created);
        }
        catch
        {
            return SingleResult<AuthModel>.Failure([("An error occurred while signing up, please check your data and try again")], HttpStatusCode.BadRequest);
        }
    }

    public async Task<SingleResult<AuthModel>> ChiefSignUp(ChiefSignUpModel model)
    {
        if (await _chiefManager.FindByEmailAsync(model.Email) is not null)
            return SingleResult<AuthModel>.Failure([("Email is already registered!")], HttpStatusCode.Conflict);

        if (!await _context.Buildings.AnyAsync(x => x.ID == model.BuildingID))
            return SingleResult<AuthModel>.Failure([("This location does not exist please try again")], HttpStatusCode.Conflict);

        /*if (await _chiefManager.FindByNameAsync(model.Username) is not null)
            return new AuthModel { Message = "Username is already registered!" };*/

        var user = new Chief
        {
            UserName = $"{model.FirstName}{model.LastName}",//model.Username,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            //MiddleName = model.MiddleName,
            PhoneNumber = model.PhoneNumber,
            //UserImage = await _imageService.Process(new ImageInput() { Content = model.Image.OpenReadStream(), FileName = Guid.NewGuid().ToString(), Path = "Images/UserImage" }),
            BuildingID = model.BuildingID,//Guid.Parse("0a6b2627-98bf-4128-9078-1124e9e05cd5"),
            //GovernmentID = model.GovernmentID,
            CreatedDate = DateTime.Now,
            IsEnabled = false,
            //LockoutEnabled = true,
            SignupDate = DateTime.Now
        };
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var result = await _chiefManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return SingleResult<AuthModel>.Failure(result.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest);
            }

            await _chiefManager.AddToRoleAsync(user, "Chief");

            var jwtSecurityToken = await CreateJwtToken(user);

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresOn = DateTime.UtcNow.AddDays(10);
            user.IsRefreshTokenRevoked = false;

            if (!await EmailConfirmation(user))
            {
            }


            await _chiefManager.UpdateAsync(user);
            await transaction.CommitAsync();
            return SingleResult<AuthModel>.Success(new AuthModel
            {
                ExpiresOn = jwtSecurityToken.ValidTo,
                Roles = ["Chief"],
                jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken,
                RefreshTokenExpiration = (DateTime)user.RefreshTokenExpiresOn,
        }, HttpStatusCode.Created);


        }
        catch
        {
            return SingleResult<AuthModel>.Failure([("An error occurred while signing up, please check your data and try again")], HttpStatusCode.BadRequest);
        }

    }

    public async Task<SingleResult<AuthModel>> GetTokenAsync(TokenRequestModel model)
    {
        var authModel = new AuthModel();

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            if (user != null) await _userManager.AccessFailedAsync(user);

            return SingleResult<AuthModel>.Failure([("Email or Password aren't correct")], HttpStatusCode.NotFound);
        }

        if (!user.IsEnabled)
        {
            return SingleResult<AuthModel>.Failure([("your applaction in under review")], HttpStatusCode.Forbidden);
        }

        if (!user.EmailConfirmed)
        {
            return SingleResult<AuthModel>.Failure([("please confrim your email")], HttpStatusCode.Conflict);
        }

        var jwtSecurityToken = await CreateJwtToken(user);
        var rolesList = await _userManager.GetRolesAsync(user);

        authModel.jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authModel.ExpiresOn = jwtSecurityToken.ValidTo;
        authModel.Roles = rolesList.ToList();

        if (user.RefreshTokenExpiresOn > DateTime.UtcNow && !user.IsRefreshTokenRevoked)
        {
            var activeRefreshToken = user.RefreshToken;
            authModel.RefreshToken = user.RefreshToken;
            authModel.RefreshTokenExpiration = (DateTime)user.RefreshTokenExpiresOn;
        }
        else
        {
            var refreshToken = GenerateRefreshToken();
            authModel.RefreshToken = refreshToken;
            authModel.RefreshTokenExpiration = DateTime.UtcNow.AddDays(10);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresOn = authModel.RefreshTokenExpiration;
            user.IsRefreshTokenRevoked = false;
            await _userManager.UpdateAsync(user);
        }

        return SingleResult<AuthModel>.Success(authModel);
    }

    public async Task<SingleResult<string>> AddRoleAsync(AddRoleModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
            return SingleResult<string>.Failure([("Invalid user ID or Role")], HttpStatusCode.NotFound);
        //return "Invalid user ID or Role";

        if (await _userManager.IsInRoleAsync(user, model.Role))
            return SingleResult<string>.Failure([("User already assigned to this role")], HttpStatusCode.Conflict);
        //return "User already assigned to this role";

        var result = await _userManager.AddToRoleAsync(user, model.Role);

        return result.Succeeded ? SingleResult<string>.Success("Role has been added", HttpStatusCode.Created) : SingleResult<string>.Failure([("Sonething went wrong")], HttpStatusCode.BadRequest);
    }

    private async Task<JwtSecurityToken> CreateJwtToken(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));

        var claims = new[]
        {
            new Claim("uid", user.Id),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("phone_number", user.PhoneNumber ?? ""),
            new Claim("photo", user.UserImage ?? "")
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }

    public async Task<SingleResult<AuthModel>> RefreshTokenAsync(string token)
    {
        var authModel = new AuthModel();

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);

        if (user == null)
        {
            //authModel.Message = "Invalid token";
            return SingleResult<AuthModel>.Failure([("Invalid token")], HttpStatusCode.BadRequest);
            //return authModel;
        }

        if (user.IsRefreshTokenRevoked)
        {
            //authModel.Message = "Inactive token";
            return SingleResult<AuthModel>.Failure([("Invalid token")], HttpStatusCode.BadRequest);
            //return authModel;
        }


        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiresOn = DateTime.UtcNow.AddDays(10);
        user.RefreshToken = newRefreshToken;
        user.IsRefreshTokenRevoked = false;
        await _userManager.UpdateAsync(user);

        var jwtToken = await CreateJwtToken(user);
        authModel.jwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        var roles = await _userManager.GetRolesAsync(user);
        authModel.ExpiresOn = jwtToken.ValidTo;
        authModel.Roles = roles.ToList();
        authModel.RefreshToken = newRefreshToken;
        authModel.RefreshTokenExpiration = user.RefreshTokenExpiresOn.Value;

        return SingleResult<AuthModel>.Success(authModel);
        //return authModel;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == token);

        if (user == null)
            return false;

        var refreshToken = user.RefreshToken;

        if (user.IsRefreshTokenRevoked || user.RefreshTokenExpiresOn < DateTime.UtcNow)
            return false;

        user.IsRefreshTokenRevoked = true;

        await _userManager.UpdateAsync(user);

        return true;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var generator = new RNGCryptoServiceProvider();

        generator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public async Task<SingleResult<Customer>> GetCustomerByID(string customerID)
    {
        var customer = await _customerManager.FindByIdAsync(customerID);
        if (customer != null) 
        {
            return SingleResult<Customer>.Success(customer);
        } 
        return SingleResult<Customer>.Failure(["This Customer does not exist"], HttpStatusCode.NotFound);

    }

    public async Task<bool> EmailCheck(string email)
    {
        return !await _context.Users.AnyAsync(x => x.Email == email.ToLower());
    }

    public async Task<bool> EmailConfirmation(User user)
    {
        if (user.EmailConfirmed)
            return false;
        try
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebUtility.UrlEncode(token);
            var url = $"Go here {token}";
            var body = $"<p>Hello: {user.FirstName} {user.LastName}</p>" +
                "<p>Please confirm your email address by clicking on the following link.</p>" +
                $"<p><a href=\"{_config["JWT:ClientUrl"]}/account/confirm-email?userID={user.Id}&token={token}\">Click here</a></p>" +
                //$"<p><a href=\"{_config["JWT:ClientUrl"]}/EmailConfirmation?userID={user.Id}&token={token}\">Click here</a></p>" +
                "<p>Thank you,</p>" +
                $"<br>Ta'am Biet";
            var emailData = new EmailData()
            {
                To = user.Email, Subject = "Confirm your email", Body = body
            };

            await _emailService.SendEmailConfirmation(emailData);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> EmailConfirmation(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;
            if (user.EmailConfirmed) return false;
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebUtility.UrlEncode(token);
            var url = $"Go here {token}";
            //var result = await _userManager.ConfirmEmailAsync(user,token);
            var body = $"<p>Hello: {user.FirstName} {user.LastName}</p>" +
                "<p>Please confirm your email address by clicking on the following link.</p>" +
                //$"<p>{token}.</p>" +
                $"<p><a href=\"{_config["JWT:ClientUrl"]}/account/confirm-email?userID={user.Id}&token={token}\">Click here</a></p>" +
                "<p>Thank you,</p>" +
                $"<br>Ta'am Beit";

            var emailData = new EmailData()
            {
                To = user.Email, Subject = "Confirm your email",Body = body
            };

            await _emailService.SendEmailConfirmation(emailData);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> SendForgotUsernameOrPasswordEmail(User user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        token = WebUtility.UrlEncode(token);
        var url = $"{_config["JWT:ClientUrl"]}/account/reset-password?token={token}&email={user.Email}";

        var body = $"<p>Hello: {user.FirstName} {user.LastName}</p>" +
           "<p>In order to reset your password, please click on the following link.</p>" +
           $"<p><a href=\"{url}\">Click here</a></p>" +
           "<p>Thank you,</p>" +
           $"<br>{_config["Email:ApplicationName"]}";

        var emailData = new EmailData()
        {
            To = user.Email,
            Subject = "Reset your password",
            Body = body
        };
        try
        {
            await _emailService.SendEmailConfirmation(emailData);
            return true;

        }
        catch
        {
            return false;
        }

    }

    public async Task<bool> EmailConfirmed(Guid userID,string token)
    {
        var user = await _userManager.FindByIdAsync(userID.ToString());

        if (user == null) return false;

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return true;
        }
        return false;
    }

    public async Task<SingleResult<bool>> PostingCustomerProfileData(PostCustomerProfileDataRequest request, byte[]? Image, string CustomerID)
    {
        if(!await _context.Buildings.AnyAsync(x => x.ID == request.BuildingID))
            return SingleResult<bool>.Failure(["wrong address"], HttpStatusCode.NotFound);
        var customer = await _context.Customers.AsTracking().FirstOrDefaultAsync(x => x.Id == CustomerID);

        if (customer == null)
            return SingleResult<bool>.Failure(["please try to login again"], HttpStatusCode.NotFound);

        if(Image != null)
        {
            var Images = await _image.Process(new ImageInput() { Content = new MemoryStream(Image), FileName = $"{CustomerID}", Path = "Images/Customer" });
            customer.UserImage = Images[1] ?? "";
        }
        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.BuildingID = request.BuildingID;
        customer.PhoneNumber = request.PhoneNumber;

        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }

    public async Task<SingleResult<GetCustomerProfileDataRequest>> GetCustomerProfileData(string CustomerID)
    {
        var customer = await _context.Customers.Where(x => x.Id == CustomerID).Select(customer => new GetCustomerProfileDataRequest()
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber ?? "",
            BuildingID = customer.BuildingID,
            StreetID = customer.Building.StreetID,
            DistrictID = customer.Building.Street.DistrictID,
            Image= customer.UserImage,
        }).FirstOrDefaultAsync();

        if (customer == null)
            return SingleResult<GetCustomerProfileDataRequest>.Failure(["please try to login again"], HttpStatusCode.NotFound);

        return SingleResult<GetCustomerProfileDataRequest>.Success(customer);
    }

    public async Task<SingleResult<bool>> SendResetPasswordEmail(string email)
    {
        //if (string.IsNullOrEmpty(email)) return BadRequest("Invalid email");

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) return SingleResult<bool>.Failure(["this email is not signed up"], HttpStatusCode.NotFound);

        if (user.EmailConfirmed == false) return SingleResult<bool>.Failure(["Please confirm your email address first"], HttpStatusCode.NotAcceptable);

        try
        {
            if (await SendForgotUsernameOrPasswordEmail(user))
            {
                return SingleResult<bool>.Success(true);
            }
            return SingleResult<bool>.Failure(["Failed to send email. Please contact admin"], HttpStatusCode.FailedDependency);
        }
        catch (Exception)
        {
            return SingleResult<bool>.Failure(["Failed to send email. Please contact admin"], HttpStatusCode.FailedDependency);
        }
    }

    public async Task<SingleResult<bool>> ResetPassword(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return SingleResult<bool>.Failure(["this email is not signed up"], HttpStatusCode.NotFound);
        if (user.EmailConfirmed == false) return SingleResult<bool>.Failure(["Please confirm your email address first"], HttpStatusCode.NotAcceptable);

        try
        {

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (result.Succeeded)
            {
                return SingleResult<bool>.Success(true);
            }
            return SingleResult<bool>.Failure(["Invalid token. Please try again"], HttpStatusCode.FailedDependency);

        }
        catch (Exception)
        {
            return SingleResult<bool>.Failure(["Invalid token. Please try again"], HttpStatusCode.FailedDependency);
        }
    }
}
