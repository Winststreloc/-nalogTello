using AnalogTrelloBE.Intefaces.IRepository;
using AnalogTrelloBE.Intefaces.IService;
using AnalogTrelloBE.Repositories;
using AnalogTrelloBE.Services;

namespace AnalogTrelloBE.Helpers;

public static class DIExtentions
{
    public static void ConfigureServices(this IServiceCollection service)
    {
        service.AddScoped<IUserService, UserService>();
        service.AddScoped<IUserRepository, UserRepository>();
        
        service.AddScoped<IPasswordHashingService, PasswordHashingService>();
        service.AddScoped<ITokenService, TokenService>();
        service.AddScoped<ICashService, CashService>();
    }
}