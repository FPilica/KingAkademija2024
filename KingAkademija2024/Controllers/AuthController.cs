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
    
    /// <summary>
    ///  Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="loginRequest">Username and password. An example valid username and password: Username: emilys | Password: emilyspass</param>
    /// <returns>A JWT token if authentication is successful. 401 Unauthorized if authentication is unsuccessful.</returns>
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