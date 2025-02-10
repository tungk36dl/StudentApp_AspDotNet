using Microsoft.EntityFrameworkCore;
using Serilog;
using StudentMngt.Api;
using StudentMngt.Application;
using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Infrastructure;
using StudentMngt.Persistence;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//service of applications
builder.Services.AddServicesApplication();


//service of infrastructure
builder.Services.AddServicesInfrastructure();
builder.Services.AddOptionsInfrastructure(builder.Configuration);

//services of persistence
builder.Services.AddSqlServerPersistence(builder.Configuration);
builder.Services.AddRepositoryUnitOfWork();

//service of Api
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
InitDatabase(app);
app.Run();

void InitDatabase(IApplicationBuilder app)
{
    using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

    var dbcontext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    dbcontext.Database.Migrate();

    var userService = serviceScope.ServiceProvider.GetRequiredService<IUserService>();
    userService.InitializeUserAdminAsync().Wait();
}
