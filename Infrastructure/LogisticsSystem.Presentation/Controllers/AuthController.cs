using LogisticsSystem.Abstraction.Services;
using LogisticsSystem.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
    {
        var authModel = await authService.RegisterAsync(registerDto);

        if (!authModel.IsAuthenticated)
            return BadRequest(authModel);

        return Ok(authModel);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
    {
        var authModel = await authService.LoginAsync(loginDto);

        if (!authModel.IsAuthenticated)
            return BadRequest(authModel);

        return Ok(authModel);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto dto)
    {
        var authModel = await authService.RefreshTokenAsync(dto);

        if (!authModel.IsAuthenticated)
            return BadRequest(authModel);

        return Ok(authModel);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync([FromBody] RefreshTokenDto dto)
    {
        var result = await authService.LogoutAsync(dto.RefreshToken);

        if (!result)
            return BadRequest(new { message = "Invalid refresh token." });

        return Ok(new { message = "Logged out successfully." });
    }
}