using AnalogTrello.Models;
using AnalogTrelloBE.Dto;
using Task = AnalogTrello.Models.Task;

namespace AnalogTrelloBE.Intefaces.IRepository;

public interface IUserRepository
{
    Task<bool> EmailOrUsernameExists(UserDto user);
    Task<User?> GetUserById(long id);
    Task<User?> GetUserByUsername(string username);
    Task<bool> InsertUser(User user);
}