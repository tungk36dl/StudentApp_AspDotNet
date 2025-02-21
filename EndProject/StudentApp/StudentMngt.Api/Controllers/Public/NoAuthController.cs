using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentMngt.Api.Controllers.Base;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Domain.InfrastructureServices;
using System.Security.Claims;

namespace Demo.Api.Controllers.Public
{
    public class NoAuthController: NoAuthorizeController
    {

        private readonly IJwtTokenService _tokenService;

        private readonly IUserService _userService;
        private readonly ILogger<NoAuthController> _logger;

        public NoAuthController(IJwtTokenService tokenService, IUserService userService, ILogger<NoAuthController> logger)
        {
            _tokenService = tokenService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<AuthorizedResponseModel> Login([FromBody] LoginViewModel model)
        {
            _logger.LogDebug("Login page");
            _logger.LogError("Login page");
            var result = await _userService.Login(model);
            return result;
        }

        [HttpPost("refresh-token")]
        public async Task<AuthorizedResponseModel> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            _logger.LogDebug("Refreshing token...");
            var result = await _userService.RefreshToken(request);
            return result;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            _logger.LogDebug("Logging out user: " + userId);
            await _userService.Logout(userId);

            return Ok(new { message = "Logged out successfully" });
        }



        [HttpPost]
        [Route("register-customer")]
        public async Task<ResponseResult> RegisterCustomer([FromBody] RegisterUserViewModel model)
        {
            var result = await _userService.RegisterCustomer(model);
            return result;
        }


    //    [HttpPost("refresh-token")]
    //    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    //    {
    //        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
    //        if (principal == null)
    //        {
    //            return Unauthorized();
    //        }

    //        var email = principal.FindFirstValue(ClaimTypes.Email);
    //        var user = await _userManager.FindByEmailAsync(email);
    //        if (user == null)
    //        {
    //            return Unauthorized();
    //        }

    //        // ✅ Lấy Refresh Token từ bảng AspNetUserTokens
    //        var savedRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, "JWT", "RefreshToken");
    //        if (savedRefreshToken == null || savedRefreshToken != request.RefreshToken)
    //        {
    //            return Unauthorized(); // Refresh Token không hợp lệ
    //        }

    //        var newClaims = new List<Claim>
    //{
    //    new("UserName", user.UserName),
    //    new(ClaimTypes.Email, user.Email)
    //};

    //        var (newAccessToken, newRefreshToken) = _tokenService.GenerateTokens(newClaims);

    //        // ✅ Cập nhật Refresh Token mới
    //        await SaveRefreshToken(user, newRefreshToken);

    //        return Ok(new
    //        {
    //            JwtToken = newAccessToken,
    //            RefreshToken = newRefreshToken
    //        });
    //    }

    //    [HttpPost("logout")]
    //    public async Task<IActionResult> Logout()
    //    {
    //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //        if (userId == null)
    //        {
    //            return Unauthorized();
    //        }

    //        var user = await _userManager.FindByIdAsync(userId);
    //        if (user == null)
    //        {
    //            return Unauthorized();
    //        }

    //        // ✅ Xóa Refresh Token khỏi bảng AspNetUserTokens
    //        await _userManager.RemoveAuthenticationTokenAsync(user, "JWT", "RefreshToken");

    //        return Ok(new { message = "Logged out successfully" });
    //    }






        [HttpGet]
        [Route("a1")]
        public string a1()
        {
            return "hello";
        }
    }
}
