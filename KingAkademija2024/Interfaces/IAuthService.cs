using KingAkademija2024.Models;

namespace KingAkademija2024.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> AuthenticateAsync(LoginRequest loginRequest);
}