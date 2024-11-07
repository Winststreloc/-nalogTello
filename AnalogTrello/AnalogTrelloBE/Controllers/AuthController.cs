using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Interfaces.IRepository;
using AnalogTrelloBE.Interfaces.IService;
using AnalogTrelloBE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AnalogTrelloBE.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly ITokenService _tokenService;

    private readonly ICashService _cashService;

    public AuthController(IUserRepository userRepository, IUserService userService,
        IPasswordHashingService passwordHashingService, ITokenService tokenService, ICashService cashService)
    {
        _userRepository = userRepository;
        _userService = userService;
        _passwordHashingService = passwordHashingService;
        _tokenService = tokenService;
        _cashService = cashService;
    }

    [HttpPost("register")]
    public async Task<ResponseDto<UserDto>> Register(UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return ResponseDto<UserDto>.Failed("Invalid username or password.");
        }

        if (!await _userRepository.EmailOrUsernameExists(userDto))
        {
            Response.StatusCode = 409;
            return ResponseDto<UserDto>.Failed("Email or Username exists");
        }

        if (!await _userService.RegisterUser(userDto))
        {
            Response.StatusCode = 500;
            return ResponseDto<UserDto>.Failed("Something went wrong...");
        }

        var user = await _userRepository.GetUserByUsername(userDto.UserName);
        await _cashService.CashingData(user.Id, user);

        Response.StatusCode = 200;
        return ResponseDto<UserDto>.Success(userDto);
    }

    [HttpPost("login")]
    public async Task<ResponseDto<Token>> Login(UserDto userDto)
    {
        var candidate = await _userRepository.GetUserByUsername(userDto.UserName);

        if (candidate == null)
        {
            Response.StatusCode = 404;
            return ResponseDto<Token>.Failed("User not found.");
        }

        if (!_passwordHashingService.VerifyHashedPassword(candidate.PasswordHash, userDto.Password))
        {
            return ResponseDto<Token>.Failed("Invalid refresh_token");
        }
        
        var userTokens = _tokenService.GenerateTokens(candidate);
        //TODO реализовать кэш токенов
        await _cashService.CashingData(candidate.Id, candidate);
        return ResponseDto<Token>.Success(userTokens);
    }

    [HttpGet("refresh-token")]
    [Authorize]
    public async Task<ResponseDto<Token>> RefreshToken([FromQuery] string refreshToken)
    {
        if (!_tokenService.ValidateRefreshToken(refreshToken))
        {
            return ResponseDto<Token>.Failed("Invalidate refresh token");
        }
        
        var userId = User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
        if (userId == null)
        {
            Response.StatusCode = 404;
            return ResponseDto<Token>.Failed("User not found");
        }

        var candidate = await _userRepository.GetUserById(long.Parse(userId));

        if (candidate == null)
        {
            return ResponseDto<Token>.Failed("User not found");
        }
            
        var userToken = _tokenService.GenerateTokens(candidate);
        return ResponseDto<Token>.Success(userToken);

    }

    [HttpPost("logout")] //TODO ебучий логаут нужно доделать, но как я понимаю это нужен редис
    public IActionResult Logout()
    {
        return Ok();
    }
}