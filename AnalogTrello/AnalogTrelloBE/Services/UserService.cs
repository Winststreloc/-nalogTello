using AnalogTrello.Models;
using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Interfaces.IRepository;
using AnalogTrelloBE.Interfaces.IService;
using AnalogTrelloBE.Models;

namespace AnalogTrelloBE.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHashingService _passwordHashingService;

    public UserService(IUserRepository repository, IPasswordHashingService passwordHashingService)
    {
        _repository = repository;
        _passwordHashingService = passwordHashingService;
    }

    public async Task<bool> RegisterUser(UserDto userDto)
    {
        var user = new User
        {
            Email = userDto.Email,
            UserName = userDto.UserName,
            PasswordHash = _passwordHashingService.HashingPassword(userDto.Password)
        };

        return await _repository.InsertUser(user);
    }
}