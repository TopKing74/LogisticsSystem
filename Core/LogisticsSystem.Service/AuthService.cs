using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LogisticsSystem.Abstraction.Services;
using LogisticsSystem.Domain.Models;
using LogisticsSystem.Domain.Models.Identity;
using LogisticsSystem.Persistence.Contexts;
using LogisticsSystem.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LogisticsSystem.Service;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<int>> roleManager,
    IConfiguration configuration,
    ApplicationDbContext context) : IAuthService
{
    public async Task<AuthModel> RegisterAsync(RegisterDto registerDto)
    {
        if (await userManager.FindByEmailAsync(registerDto.Email) is not null)
            return new AuthModel { Message = "Email is already registered!" };

        if (await userManager.FindByNameAsync(registerDto.UserName) is not null)
            return new AuthModel { Message = "Username is already taken!" };

        var user = new ApplicationUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthModel { Message = errors };
        }

        await userManager.AddToRoleAsync(user, "Customer");

        var roles = await userManager.GetRolesAsync(user);
        var jwtSecurityToken = CreateJwtToken(user, roles);
        var refreshToken = GenerateRefreshToken();

        context.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        });

        await context.SaveChangesAsync();

        return new AuthModel
        {
            Message = "Registration successful",
            IsAuthenticated = true,
            Username = user.UserName!,
            Email = user.Email!,
            Roles = roles.ToList(),
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            ExpiresOn = jwtSecurityToken.ValidTo,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthModel> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            return new AuthModel { Message = "Email or Password is incorrect!" };

        var roles = await userManager.GetRolesAsync(user);
        var jwtSecurityToken = CreateJwtToken(user, roles);
        var refreshToken = GenerateRefreshToken();

        context.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        });

        await context.SaveChangesAsync();

        return new AuthModel
        {
            Message = "Login successful",
            IsAuthenticated = true,
            Username = user.UserName!,
            Email = user.Email!,
            Roles = roles.ToList(),
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            ExpiresOn = jwtSecurityToken.ValidTo,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthModel> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var storedToken = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken);

        if (storedToken is null || storedToken.ExpiresAt < DateTime.UtcNow)
            return new AuthModel { Message = "Invalid or expired refresh token." };

        var user = await userManager.FindByIdAsync(storedToken.UserId.ToString());
        if (user is null)
            return new AuthModel { Message = "User not found." };

        context.RefreshTokens.Remove(storedToken);

        var roles = await userManager.GetRolesAsync(user);
        var jwtSecurityToken = CreateJwtToken(user, roles);
        var newRefreshToken = GenerateRefreshToken();

        context.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        });

        await context.SaveChangesAsync();

        return new AuthModel
        {
            Message = "Token refreshed",
            IsAuthenticated = true,
            Username = user.UserName!,
            Email = user.Email!,
            Roles = roles.ToList(),
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            ExpiresOn = jwtSecurityToken.ValidTo,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var storedToken = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedToken is null)
            return false;

        context.RefreshTokens.Remove(storedToken);
        await context.SaveChangesAsync();
        return true;
    }

    private JwtSecurityToken CreateJwtToken(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));

        var token = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}