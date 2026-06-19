using LogisticsSystem.Shared.Dtos;

namespace LogisticsSystem.Abstraction.Services;

public interface IAuthService
{
    Task<AuthModel> RegisterAsync(RegisterDto registerDto);
    Task<AuthModel> LoginAsync(LoginDto loginDto);
    Task<AuthModel> RefreshTokenAsync(RefreshTokenDto dto);
    Task<bool> LogoutAsync(string refreshToken);
}