using AnalogTrello.Models;
using AnalogTrelloBE.Dto;

namespace AnalogTrelloBE.Intefaces.IService;

public interface IUserService
{
    Task<bool> RegisterUser(UserDto userDto);
}