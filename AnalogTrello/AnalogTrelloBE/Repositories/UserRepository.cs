using AnalogTrello.Data;
using AnalogTrello.Models;
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
        return !emailExist && !userNameExist;
    }

    public async Task<User?> GetUserById(long id)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<bool> InsertUser(User user)
    {
        await _context.Users.AddAsync(user);
        return await _context.SaveChangesAsync() > 0;
    }
}
