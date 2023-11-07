using System.Text.Json;
using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Intefaces.IRepository;
using AnalogTrelloBE.Intefaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;


namespace AnalogTrelloBE.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private ResponseDto? _responseDto;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly ITokenService _tokenService;

    private ICashService _cashService;

    public AuthController(IUserRepository userRepository, IUserService userService,
        IPasswordHashingService passwordHashingService, ITokenService tokenService, ICashService cashService)
    {
        _userRepository = userRepository;
        _userService = userService;
        _passwordHashingService = passwordHashingService;
        _tokenService = tokenService;
        _cashService = cashService;
        _responseDto = new ResponseDto();
    }

    [HttpPost("register")]
    public async Task<ResponseDto> Register(UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            _responseDto.ErrorMessages = new List<string> { "Invalid username or password." };
            _responseDto.IsSuccess = false;
            return _responseDto;
        }

        if (!await _userRepository.EmailOrUsernameExists(userDto))
        {
            Response.StatusCode = 409;
            Response.ContentType = "application/json";
            _responseDto.ErrorMessages = new List<string> { "Email or Username exists" };
            _responseDto.IsSuccess = false;
            return _responseDto;
        }

        if (!await _userService.RegisterUser(userDto))
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string> { "Something went wrong..." };
            Response.StatusCode = 500;
        }

        var user = await _userRepository.GetUserByUsername(userDto.UserName);
        await _cashService.CashingData(user.Id, user);

        _responseDto.IsSuccess = true;
        _responseDto.Result = "User is registered";
        Response.StatusCode = 200;
        return _responseDto;
    }

    [HttpPost("login")]
    public async Task<ResponseDto> Login(UserDto userDto)
    {
        var candidate = await _userRepository.GetUserByUsername(userDto.UserName);

        if (candidate == null)
        {
            Response.StatusCode = 404;
            Response.ContentType = "application/json";
            _responseDto.ErrorMessages = new List<string> { "User not found" };
            _responseDto.IsSuccess = false;
            return _responseDto;
        }

        if (!_passwordHashingService.VerifyHashedPassword(candidate.PasswordHash, userDto.Password))
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string> { "Invalid refresh_token" };
            return _responseDto;
        }
        
        var userTokens = _tokenService.GenerateTokens(candidate);
        //TODO реализовать кэш токенов
        await _cashService.CashingData(candidate.Id, candidate);
        _responseDto.Result = userTokens;
        return _responseDto;
    }

    [HttpGet("refresh-token")]
    [Authorize]
    public async Task<ResponseDto> RefreshToken([FromQuery] string refreshToken)
    {
        if (_tokenService.ValidateRefreshToken(refreshToken))
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (userId == null)
            {
                Response.StatusCode = 404;
                Response.ContentType = "application/json";
                _responseDto.ErrorMessages = new List<string> { "User not found" };
                _responseDto.IsSuccess = false;
                return _responseDto;
            }

            var candidate = await _userRepository.GetUserById(Int64.Parse(userId));

            var userTokens = _tokenService.GenerateTokens(candidate);
            _responseDto.Result = userTokens;
            return _responseDto;
        }

        _responseDto.IsSuccess = false;
        _responseDto.ErrorMessages = new List<string> { "Invalid refresh_token" };
        return _responseDto;
    }

    [HttpPost("logout")] //TODO ебучий логаут нужно доделать, но как я понимаю это нужен редис
    public IActionResult Logout()
    {
        return Ok();
    }
}