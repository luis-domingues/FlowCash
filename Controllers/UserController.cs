using FlowCash.Models;
using FlowCash.Serivces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ResetPasswordRequest = FlowCash.DTOs.ResetPasswordRequest;

namespace FlowCash.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserServices _services;

    public UserController(UserServices services)
    {
        _services = services;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        try
        {
            var newUser = await _services.RegisterUserAsync(user);
            return CreatedAtAction(nameof(Register), new { id = newUser.Id }, newUser);
        }
        catch (InvalidOperationException message)
        {
            return BadRequest(new { message = message.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno");
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _services.ValidateUserAsync(request.Email, request.Password);
        if (token == null) return Unauthorized(new { message = "Email ou senha inválidos." });
        
        return Ok(new { token });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var token = await _services.GeneratePasswordResetTokenAsync(request.Email);
        if (token == null) return BadRequest(new { message = "E-mail não encontrado." });

        return Ok(new { message = "Link de recuperação será enviado ao e-mail" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var success = await _services.ResetPasswordAsync(request.Token, request.NewPassword);
        if(!success) return BadRequest(new { message = "Token inválido ou expirado." });
        
        return Ok(new { message = "Senha redefinida com sucesso!" });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var users = await _services.GetAllUsersAsync();
        return Ok(users);
    }
}