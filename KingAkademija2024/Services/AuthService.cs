using KingAkademija2024.Helpers;
using KingAkademija2024.Interfaces;
using KingAkademija2024.Models;

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

        var token = JwtHelper.GenerateJwtToken(user, _configuration);
        return new LoginResponse { Token = token };
    }
}