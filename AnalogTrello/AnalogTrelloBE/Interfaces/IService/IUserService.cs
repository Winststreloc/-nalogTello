using AnalogTrelloBE.Dto;

namespace AnalogTrelloBE.Interfaces.IService;

public interface IUserService
{
    Task<bool> RegisterUser(UserDto userDto);
}