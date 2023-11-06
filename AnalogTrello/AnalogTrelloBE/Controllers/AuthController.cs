using System.Net;
using AnalogTrello.Models;
using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Intefaces.IRepository;
using Microsoft.AspNetCore.Mvc;
using Task = AnalogTrello.Models.Task;

namespace AnalogTrelloBE.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{

    private readonly IUserRepository _userRepository;
    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!await _userRepository.EmailOrUsernameExists(userDto))
        {
            Response.StatusCode = 409;
            Response.ContentType = "application/json";
            Response.HttpContext.
            Response.Write("This email is already taken");
            Response.End();
        }
    }     
}