using AnalogTrelloBE.Interfaces.IRepository;
using AnalogTrelloBE.Interfaces.IService;
using AnalogTrelloBE.Repositories;
using AnalogTrelloBE.Services;
using BuildingBlocks;

namespace AnalogTrelloBE.Helpers;

public static class DiExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskSchedulerRepository, TaskSchedulerSchedulerRepository>();
        
        services.AddScoped<IPasswordHashingService, PasswordHashingService>();
        services.AddScoped<ITokenService, TokenService>();
        
        services.AddRedis();
    }
}