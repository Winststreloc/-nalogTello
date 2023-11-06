using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Intefaces.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AnalogTrelloBE.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private ResponseDto? _responseDto;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _responseDto = new ResponseDto();
    }

    [HttpGet("$id")]
    public async Task<ResponseDto> GetUser(long userId)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = 404;
        }

        var user = _mapper.Map<UserDto>(await _userRepository.GetUserById(userId));
        if (user == null)
        {
            _responseDto.ErrorMessages = new List<string> { "User not found" };
            _responseDto.IsSuccess = false;
            Response.StatusCode = 404;
            return _responseDto;
        }

        _responseDto.Result = user;
        _responseDto.IsSuccess = true;
        Response.StatusCode = 200;
        return _responseDto;
    }
}