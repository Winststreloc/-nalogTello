using System.Text.Json;
using AnalogTrello.Models;
using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Intefaces.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace AnalogTrelloBE.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private ResponseDto? _responseDto;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private IDistributedCache cache;

    public UserController(IUserRepository userRepository, IMapper mapper, IDistributedCache distributedCache)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        cache = distributedCache;
        _responseDto = new ResponseDto();
    }

    [HttpGet("$id")]
    public async Task<ResponseDto> GetUser(long userId)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = 404;
        }

        //TODO тут кароче нужно придумать как работать с кэшом, а именно уменя проблемы, нужно чтобы везде в Redis шёл один и 
        //todo тот же тип данных, а именно UserDto и Token
        UserDto? user = null;
        var userString = await cache.GetStringAsync(userId.ToString());
        if (userString != null) user = JsonSerializer.Deserialize<UserDto>(userString);

        if (user == null)
        {
            user = _mapper.Map<UserDto>(await _userRepository.GetUserById(userId));
            if (user == null)
            {
                _responseDto.ErrorMessages = new List<string> { "User not found" };
                _responseDto.IsSuccess = false;
                Response.StatusCode = 404;
                return _responseDto;
            }

            userString = JsonSerializer.Serialize(user);
            await cache.SetStringAsync(userId.ToString(), userString, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });
        }

        _responseDto.Result = user;
        _responseDto.IsSuccess = true;
        Response.StatusCode = 200;
        return _responseDto;
    }
}