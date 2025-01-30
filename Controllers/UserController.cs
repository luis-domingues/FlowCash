using FlowCash.Models;
using FlowCash.Serivces;
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
    
}