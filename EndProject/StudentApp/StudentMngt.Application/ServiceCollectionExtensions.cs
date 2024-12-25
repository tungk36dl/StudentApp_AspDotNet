using StudentMngt.Application.Services;
using StudentMngt.Domain.ApplicationServices.Users;
using Microsoft.Extensions.DependencyInjection;

namespace StudentMngt.Application;

public static class ServiceCollectionExtensions
{
    public static void AddServicesApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }

}