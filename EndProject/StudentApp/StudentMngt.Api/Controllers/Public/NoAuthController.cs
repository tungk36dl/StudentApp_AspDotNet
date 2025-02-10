using Microsoft.AspNetCore.Mvc;
using StudentMngt.Api.Controllers.Base;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.Users;

namespace Demo.Api.Controllers.Public
{
    public class NoAuthController: NoAuthorizeController
    {
        private readonly IUserService _userService;
        private readonly ILogger<NoAuthController> _logger;
        public NoAuthController(IUserService userService, ILogger<NoAuthController> logger)
        {
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

        [HttpPost]
        [Route("register-customer")]
        public async Task<ResponseResult> RegisterCustomer([FromBody] RegisterUserViewModel model)
        {
            var result = await _userService.RegisterCustomer(model);
            return result;
        }

        [HttpGet]
        [Route("a1")]
        public string a1()
        {
            return "hello";
        }
    }
}
