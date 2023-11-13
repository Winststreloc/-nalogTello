using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using AnalogTrello.Models;
using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Intefaces.IService;
using Microsoft.Extensions.Caching.Distributed;
using Task = System.Threading.Tasks.Task;

namespace AnalogTrelloBE.Helpers;

public class CashService : ICashService
{
    private IDistributedCache cache;
    private ITokenService _tokenService;

    public CashService(IDistributedCache cache, ITokenService tokenService)
    {
        this.cache = cache;
        _tokenService = tokenService;
    }

    public async Task CashingData(long id, object data)
    {
        var token = _tokenService.GenerateTokens(JsonSerializer.Deserialize<User>(data.ToString()));
        var value = new
        {
            userData = data,
            tokens = token
        };
        
        await cache.SetStringAsync(id.ToString(), JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        });
    }
}