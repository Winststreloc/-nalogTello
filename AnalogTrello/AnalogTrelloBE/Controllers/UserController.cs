using System.Text.Json;
using AnalogTrello.Models;
using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Interfaces.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace AnalogTrelloBE.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IDistributedCache _cache;

    public UserController(IUserRepository userRepository, IMapper mapper, IDistributedCache distributedCache)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _cache = distributedCache;
    }

    [HttpGet("$id")]
    public async Task<ResponseDto<UserDto>> GetUser(long userId)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = 404;
        }

        //TODO тут кароче нужно придумать как работать с кэшом, а именно уменя проблемы, нужно чтобы везде в Redis шёл один и 
        //todo тот же тип данных, а именно UserDto и Token
        UserDto? user = null;
        
        var userString = await _cache.GetStringAsync(userId.ToString());
        
        if (userString != null)
        {
            user = JsonSerializer.Deserialize<UserDto>(userString);
        }

        if (user == null)
        {
            user = _mapper.Map<UserDto>(await _userRepository.GetUserById(userId));
            if (user == null)
            {
                Response.StatusCode = 404;
                return ResponseDto<UserDto>.Failed("User not found");
            }

            userString = JsonSerializer.Serialize(user);
            await _cache.SetStringAsync(userId.ToString(), userString, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });
        }
        
        Response.StatusCode = 200;
        return ResponseDto<UserDto>.Success(user);
    }
}