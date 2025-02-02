using System.Security.Cryptography;
using FlowCash.Data;
using FlowCash.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowCash.Serivces;

public class UserServices
{
    private readonly FlowCashContext _context;
    private readonly TokenServices _tokenServices;

    public UserServices(FlowCashContext context, TokenServices tokenServices)
    {
        _context = context;
        _tokenServices = tokenServices;
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
        
        return _tokenServices.GenerateToken(user.Email);
    }

    //recovery token
    public async Task<string?> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return null;
        
        //generate 32 byte token and convert to string base64
        var token = RandomNumberGenerator.GetBytes(32);
        var resetToken = Convert.ToBase64String(token);

        user.ResetToken = resetToken;
        user.ResetTokenExpiration = DateTime.UtcNow.AddMinutes(30);

        await _context.SaveChangesAsync();
        return resetToken;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiration > DateTime.UtcNow);
        if (user == null) return false;

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.ResetToken = null;
        user.ResetTokenExpiration = null;

        await _context.SaveChangesAsync();
        return true;
    }

    //get all users
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}