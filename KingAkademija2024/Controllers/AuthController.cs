using KingAkademija2024.Interfaces;
using KingAkademija2024.Models;
using Microsoft.AspNetCore.Mvc;

namespace KingAkademija2024.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var response = await _authService.AuthenticateAsync(loginRequest);
        if (response == null)
        {
            return Unauthorized("Invalid username or password");
        }

        return Ok(response);
    }
}