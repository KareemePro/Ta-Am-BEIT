using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Services.Auth;
using FoodDelivery.Models.DominModels.Auth;
using FoodDelivery.Services.Emails;
using FoodDelivery.Models.DTO.CustomerDTO;
using FoodDelivery.Models.DTO.AuthDTO;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication5.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    private readonly IEmailService _emailService;

    public AuthController(IAuthService authService, IEmailService emailService)
    {
        _authService = authService;

        _emailService = emailService;
    }

    [HttpPost("CustomerSignUp")]
    [ProducesResponseType(typeof(AuthModel), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]

    public async Task<IActionResult> CustomerSignUp([FromBody] CustomerSignUpModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _authService.CustomerSignUp(model);

        if (!result.IsSuccess)
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };

        SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpiration);
        return Ok(result.Data);
    }

    [HttpPost("ChiefSignUp")]
    [ProducesResponseType(typeof(AuthModel), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]

    public async Task<IActionResult> ChiefSignUp([FromForm] ChiefSignUpModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _authService.ChiefSignUp(model);

        if (!result.IsSuccess)
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };

        SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpiration);
        return Ok(result.Data);
    }

    [HttpPost("Login")]
    [ProducesResponseType(typeof(AuthModel), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 403)]
    [ProducesResponseType(typeof(List<string>), 409)]
    public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
    {
        var result = await _authService.GetTokenAsync(model);

        if (!result.IsSuccess) return new ObjectResult(result.Errors)
        {
            StatusCode = (int)result.HttpStatusCode
        };


        if (!string.IsNullOrEmpty(result.Data.RefreshToken))
            SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpiration);

        return Ok(result.Data);
    }

    [HttpPost("addRole")]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]

    public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.AddRoleAsync(model);

        if (!string.IsNullOrEmpty(result.Data)) return new ObjectResult(result.Errors)
        {
            StatusCode = (int)result.HttpStatusCode
        };

        return Created();
    }

    [HttpGet("refreshToken")]
    [ProducesResponseType(typeof(AuthModel), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]

    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await _authService.RefreshTokenAsync(refreshToken);

        if (!result.IsSuccess) return new ObjectResult(result.Errors)
        {
            StatusCode = (int)result.HttpStatusCode
        };

        SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpiration);

        return Ok(result.Data);
    }

    [HttpGet("revokeToken")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 404)]

    public async Task<IActionResult> RevokeToken()
    {
        var token = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest("Token is required!");

        var result = await _authService.RevokeTokenAsync(token);

        if (!result)
            return NotFound("Token is invalid!");

        return NoContent();
    }
    [HttpPost("CheckEmail")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> CheckEmail([FromBody] Email email)
    {
        var EmailDoesNotExist = await _authService.EmailCheck(email.email);

        if (EmailDoesNotExist) return Ok(true);

        return Ok(false);
    }

    [HttpPost("ResendEmailConfirmation/{email}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(bool), 400)]

    public async Task<IActionResult> ResendEmailConfirmation(string email)
    {
        if (string.IsNullOrEmpty(email)) return BadRequest("Invalid email");

        var result = await _authService.EmailConfirmation(email);

        if (result) return Ok(true);

        return BadRequest(false);

    }
    [HttpPost("CustomerProfile")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(void), 404)]
    [ProducesResponseType(typeof(string[]), 401)]
    [ProducesResponseType(typeof(void), 201)]
    public async Task<IActionResult> PostingCustomerProfile(PostCustomerProfileDataRequest request)
    {

        byte[]? bytes = null;
        var base64 = request.Image;
        if (base64 != null)
        {
            base64 = base64.Replace("data:image/jpeg;base64,", "");
            bytes = Convert.FromBase64String(base64);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var CustomerID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        var result = await _authService.PostingCustomerProfileData(request, bytes, CustomerID.ToString());
        if (result.IsSuccess)
        {
            return Accepted();
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }

    }

    [HttpGet("CustomerProfile")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(void), 404)]
    [ProducesResponseType(typeof(string[]), 401)]
    [ProducesResponseType(typeof(GetCustomerProfileDataRequest), 200)]
    public async Task<IActionResult> GetCustomerProfile()
    {
        var uidClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid");
        var CustomerID = uidClaim != null ? Guid.Parse(uidClaim.Value) : Guid.Empty;

        var result = await _authService.GetCustomerProfileData(CustomerID.ToString());
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }
    }

    [HttpGet("EmailConfirmation")]
    [ProducesResponseType(typeof(bool), 204)]
    [ProducesResponseType(typeof(bool), 404)]
    public async Task<IActionResult> EmailConfirmation([FromQuery] Guid userID, [FromQuery] string token)
    {
        var EmailConfirmed = await _authService.EmailConfirmed(userID, token);

        if (EmailConfirmed) return NoContent();

        return NotFound();
    }

    [HttpPut("ResetPassword")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(string[]), 406)]
    [ProducesResponseType(typeof(string[]), 404)]
    [ProducesResponseType(typeof(string[]), 424)]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var result = await _authService.ResetPassword(request);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }

    }

    [HttpPost("ForgotPassword/{email}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(string[]), 406)]
    [ProducesResponseType(typeof(string[]), 404)]
    [ProducesResponseType(typeof(string[]), 424)]
    public async Task<IActionResult> ForgotUsernameOrPassword(string email)
    {
        if (string.IsNullOrEmpty(email)) return BadRequest("Invalid email");

        var result = await _authService.SendResetPasswordEmail(email);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            return new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.HttpStatusCode
            };
        }
    }
    

    private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime(),
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

}
