using FlowCash.Data;
using FlowCash.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowCash.Serivces;

public class UserServices
{
    private readonly FlowCashContext _context;
    private readonly TokenService _tokenService;

    public UserServices(FlowCashContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
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

    public async Task<string?> ValidateUserAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password)) return null;
        
        return _tokenService.GenerateToken(user.Email);
    }

    //get all users
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}