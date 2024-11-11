using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Options;

public static class ConfigurationExtensions
{
    public static TModel AddValidateOptions<TModel>(this IServiceCollection service) where TModel : class, new()
    {
        service.AddOptions<TModel>()
            .BindConfiguration(typeof(TModel).Name)
            .ValidateDataAnnotations();
        var options = service.BuildServiceProvider().GetRequiredService<IOptions<TModel>>().Value;
        service.AddSingleton(options);
        
        return options; 
    }

    public static TModel GetOptions<TModel>(this IServiceCollection service, string section) where TModel : new()
    {
        var model = new TModel();
        var configuration = service.BuildServiceProvider().GetService<IConfiguration>();
        configuration?.GetSection(section).Bind(model);
        
        return model;
    }
    public static TModel GetOptions<TModel>(this IServiceCollection service) where TModel : new()
    {
        var options = service.BuildServiceProvider().GetService<TModel>();
        
        return options ?? new TModel();
    }
}