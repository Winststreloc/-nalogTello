using BuildingBlocks.Cache;
using BuildingBlocks.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace BuildingBlocks;

public static class RedisExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        IWebHostEnvironment env = serviceProvider.GetRequiredService<IWebHostEnvironment>();

        if (env.IsEnvironment("Local"))
        {
            services.AddDistributedMemoryCache();
            return services;
        }

        services.AddValidateOptions<RedisOptions>();
        RedisOptions redisOptions = services.BuildServiceProvider().GetRequiredService<RedisOptions>();

        var multiplexer = ConnectionMultiplexer.Connect(redisOptions.ConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        services.AddScoped<IRedisCacheService, RedisCacheService>();

        ConfigurationOptions options = new ConfigurationOptions
        {
            ConnectRetry = 5,
            ConnectTimeout = 5000,
            KeepAlive = 2,
            User = redisOptions.User,
            Password = redisOptions.Password
        };
        options.EndPoints.Add(redisOptions.ConnectionString);

        services.AddStackExchangeRedisCache(x => { x.ConfigurationOptions = options; });

        return services;
    }
}