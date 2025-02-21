using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Domain.Exceptions;
using StudentMngt.Domain.Utility;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Demo.Api.Controllers.Public;
using System.Threading.Tasks;

namespace StudentMngt.Api.Filters
{
    public class ApplicationAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly ILogger<NoAuthController> _logger;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
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
                    // Sử dụng await thay vì .Result để tránh deadlock
                    var user = await userService.GetUserProfile(userName);
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
                catch (Exception)
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
