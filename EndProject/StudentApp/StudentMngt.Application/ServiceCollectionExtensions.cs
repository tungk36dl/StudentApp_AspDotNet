using StudentMngt.Application.Services;
using StudentMngt.Domain.ApplicationServices.Users;
using Microsoft.Extensions.DependencyInjection;
using StudentMngt.Domain.ApplicationServices.SubjectAS;
using StudentMngt.Domain.ApplicationServices.SubjectDetailAS;
using StudentMngt.Domain.ApplicationServices.ClassesAS;
using StudentMngt.Domain.ApplicationServices.CohortAS;
using StudentMngt.Domain.ApplicationServices.MajorAS;

namespace StudentMngt.Application;

public static class ServiceCollectionExtensions
{
    public static void AddServicesApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<ISubjectDetailService, SubjectDetailService>();
        services.AddScoped<IClassesService, ClassesService>();
        services.AddScoped<ICohortService, CohortService>();
        services.AddScoped<IMajorService, MajorService>();
    }

}