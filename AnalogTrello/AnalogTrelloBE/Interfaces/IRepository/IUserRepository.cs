using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Models;

namespace AnalogTrelloBE.Interfaces.IRepository;

public interface IUserRepository
{
    Task<bool> EmailOrUsernameExists(UserDto user);
    Task<User?> GetUserById(long id);
    Task<User?> GetUserByUsername(string username);
    Task<bool> InsertUser(User user);
}