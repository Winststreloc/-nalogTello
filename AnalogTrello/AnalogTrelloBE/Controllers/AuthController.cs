using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Interfaces.IRepository;
using AnalogTrelloBE.Interfaces.IService;
using AnalogTrelloBE.Models;
using BuildingBlocks.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AnalogTrelloBE.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
    IUserRepository userRepository,
    IUserService userService,
    IPasswordHashingService passwordHashingService,
    ITokenService tokenService,
    IRedisCacheService cashService)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<ResponseDto<UserDto>> Register(UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return ResponseDto<UserDto>.Failed("Invalid username or password.");
        }

        if (!await userRepository.EmailOrUsernameExists(userDto))
        {
            Response.StatusCode = 409;
            return ResponseDto<UserDto>.Failed("Email or Username exists");
        }

        if (!await userService.RegisterUser(userDto))
        {
            Response.StatusCode = 500;
            return ResponseDto<UserDto>.Failed("Something went wrong...");
        }

        var user = await userRepository.GetUserByUsername(userDto.UserName);

        if (user == null)
        {
            Response.StatusCode = 404;
            return ResponseDto<UserDto>.Failed("Not found user.");
        }
        await cashService.SetAsync(user.Id.ToString(), user);

        Response.StatusCode = 200;
        return ResponseDto<UserDto>.Success(userDto);
    }

    [HttpPost("login")]
    public async Task<ResponseDto<Token>> Login(UserDto userDto)
    {
        var candidate = await userRepository.GetUserByUsername(userDto.UserName);

        if (candidate == null)
        {
            Response.StatusCode = 404;
            return ResponseDto<Token>.Failed("User not found.");
        }

        if (!passwordHashingService.VerifyHashedPassword(candidate.PasswordHash, userDto.Password))
        {
            return ResponseDto<Token>.Failed("Invalid refresh_token");
        }
        
        var userTokens = tokenService.GenerateTokens(candidate);
        
        await cashService.SetAsync(candidate.Id.ToString(), candidate);
        
        return ResponseDto<Token>.Success(userTokens);
    }

    [HttpGet("refresh-token")]
    [Authorize]
    public async Task<ResponseDto<Token>> RefreshToken([FromQuery] string refreshToken)
    {
        if (!tokenService.ValidateRefreshToken(refreshToken))
        {
            return ResponseDto<Token>.Failed("Invalidate refresh token");
        }
        
        var userId = User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
        if (userId == null)
        {
            Response.StatusCode = 404;
            return ResponseDto<Token>.Failed("User not found");
        }

        var candidate = await userRepository.GetUserById(long.Parse(userId));

        if (candidate == null)
        {
            return ResponseDto<Token>.Failed("User not found");
        }
            
        var userToken = tokenService.GenerateTokens(candidate);
        return ResponseDto<Token>.Success(userToken);

    }

    [HttpPost("logout")] //TODO ебучий логаут нужно доделать, но как я понимаю это нужен редис
    public IActionResult Logout()
    {
        return Ok();
    }
}