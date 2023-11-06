using AnalogTrello.Data;
using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Intefaces.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AnalogTrelloBE.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ToDoDbContext _context;

    public UserRepository(ToDoDbContext context)
    {
        _context = context;
    }

    public async Task<bool> EmailOrUsernameExists(UserDto user)
    {
        var emailExist = await _context.Users.AnyAsync(u => u.Email == user.Email);
        var userNameExist = await _context.Users.AnyAsync(u => u.UserName == user.UserName);
        return emailExist && userNameExist;
    }
}
