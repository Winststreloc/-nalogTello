using AnalogTrelloBE.Dto;
using Task = AnalogTrello.Models.Task;

namespace AnalogTrelloBE.Intefaces.IRepository;

public interface IUserRepository
{
    Task<bool> EmailOrUsernameExists(UserDto user);
}