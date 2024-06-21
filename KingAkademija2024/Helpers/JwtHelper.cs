using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KingAkademija2024.Models;
using Microsoft.IdentityModel.Tokens;

namespace KingAkademija2024.Helpers;

public class JwtHelper
{
    public static string GenerateJwtToken(User user, IConfiguration configuration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
        
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