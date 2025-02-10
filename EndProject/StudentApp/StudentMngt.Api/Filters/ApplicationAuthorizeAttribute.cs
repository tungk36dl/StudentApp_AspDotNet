using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Domain.Exceptions;
using StudentMngt.Domain.Utility;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Demo.Api.Controllers.Public;

namespace StudentMngt.Api.Filters
{
    public class ApplicationAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly ILogger<NoAuthController> _logger;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userName = context.HttpContext.User.Claims?
                        .FirstOrDefault(i => i.Type == "UserName")?.Value ?? string.Empty;

            if (string.IsNullOrEmpty(userName))
            {
                throw new UserException.UnauthorizedException();
            }

            var userService = (IUserService?)context.HttpContext.RequestServices.GetService(typeof(IUserService));
            if (userService != null)
            {

                try
                {
                    // Sử dụng .Result là 1 thuộc tính của Task giúp đồng bộ hóa luồng thực thi cho đến khi luồng bất đồng bộ hoàn thành và trả về dữ liệu 
                    var user = userService.GetUserProfile(userName).Result;
                    if (user != null)
                    {
                        var currentUser = new UserProfileModel()
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            Email = user.Email,
                            Permissions = user.Permissions
                        };

                        context.HttpContext.Request.Headers.Add(CommonConstants.Header.CurrentUser, JsonConvert.SerializeObject(currentUser));
                    }
                    else
                    {
                        throw new UserException.UserNotFoundException();
                    }
                }
                catch (Exception ex)
                {
                    throw new UserException.UserNotFoundException();
                }
            }
            else
            {
                throw new UserException.UserNotFoundException();
            }
        }
    }
}