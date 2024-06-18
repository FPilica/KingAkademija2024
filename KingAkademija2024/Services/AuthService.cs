using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KingAkademija2024.Interfaces;
using KingAkademija2024.Models;
using Microsoft.IdentityModel.Tokens;

namespace KingAkademija2024.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AuthService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<LoginResponse> AuthenticateAsync(LoginRequest loginRequest)
    {
        var response = await _httpClient.GetFromJsonAsync<UserApiResponse>("https://dummyjson.com/users");
        var user = response.Users.FirstOrDefault(user =>
            user.Username == loginRequest.Username && user.Password == loginRequest.Password);

        if (user == null)
        {
            return null;
        }

        var token = GenerateJwtToken(user);
        return new LoginResponse { Token = token };
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}