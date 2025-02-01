using FlowCash.Models;
using FlowCash.Serivces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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
        if (token == null)
        {
            return Unauthorized(new { message = "Email ou senha inválidos." });
        }

        return Ok(new { token });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var users = await _services.GetAllUsersAsync();
        return Ok(users);
    }
}