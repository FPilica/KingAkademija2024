using KingAkademija2024.Interfaces;
using KingAkademija2024.Models;
using Microsoft.AspNetCore.Mvc;

namespace KingAkademija2024.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        _logger.LogInformation("Login attempt: {Username}", loginRequest.Username);
        
        var response = await _authService.AuthenticateAsync(loginRequest);
        if (response == null)
        {
            _logger.LogWarning("Login failed: {Username}", loginRequest.Username);
            return Unauthorized("Invalid username or password");
        }
        
        _logger.LogInformation("Login successful: {Username}", loginRequest.Username);
        return Ok(response);
    }
}