using StudentMngt.Domain.InfrastructureServices;
using StudentMngt.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StudentMngt.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddServicesInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
              
    }

    public static void AddOptionsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOption>(configuration.GetSection("JwtOption"));
    }

    public static void AddRedisInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var connectionString = configuration["ConnectionStrings:Redis"];
            redisOptions.Configuration = connectionString;
        });
    }
}