using FlowCash.Data;
using FlowCash.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowCash.Serivces;

public class UserServices
{
    private readonly FlowCashContext _context;

    public UserServices(FlowCashContext context)
    {
        _context = context;
    }
    
    //register user 
    public async Task<User?> RegisterUserAsync(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null) throw new InvalidOperationException("Email registrado!");

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> ValidateUserAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;

        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }

    //get all users
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}