using Stickers.Models.Dtos;
using Stickers.Models.Entities;
using Stickers.Models.Exceptions;
using Stickers.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Stickers.Services;

public class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration configuration,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<RegisterResponseDto> Register(RegisterDto registerDto)
    {
        logger.LogInformation("Processing sign up for user: {Username}", registerDto.Username);

        var existingUser = await userManager.FindByNameAsync(registerDto.Username);
        if (existingUser != null)
        {
            logger.LogWarning("Username already exists: {Usuario}", registerDto.Username);
            throw new UsernameAlreadyExistsException($"Username already exists: {registerDto.Username}");
        }

        var user = new User
        {
            UserName = registerDto.Username,
            Role = registerDto.Role.ToLower(),
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(erro => erro.Description));
            logger.LogWarning("Error registering user {Usename}: {Erros}", registerDto.Username, errors);
            throw new IdentityRegistrationException($"Error registering user {registerDto.Username}: {errors}");
        }

        logger.LogInformation("Sign up successful for user: {Username}", registerDto.Username);

        return new RegisterResponseDto
        {
            Success = true,
            Message = "User registered successfully",
            Username = registerDto.Username,
            Role = registerDto.Role
        };
    }

    public async Task<LoginResponseDto?> Login(LoginDto loginDto)
    {
        logger.LogInformation("Processing login for user: {Username}", loginDto.Username);

        var user = await userManager.FindByNameAsync(loginDto.Username);
        if (user == null)
        {
            logger.LogWarning("User not found: {Username}", loginDto.Username);
            return null;
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            logger.LogWarning("Wrong password for user: {Username}", loginDto.Username);
            return null;
        }

        var token = GenerateJwtToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        logger.LogInformation("Login successful for user: {Username} with role: {Role}", user.UserName, user.Role);

        return new LoginResponseDto
        {
            Token = token,
            Role = user.Role,
            Username = user.UserName ?? "", // TODO check null username
            ExpiresAt = expiresAt
        };
    }

    public string GenerateJwtToken(User user)
    {
        var jwtKey = configuration["Jwt:Key"] ?? "Q6BdpuDcgep7%L3%7DFVPna@bmrfikW4";
        var key = Encoding.ASCII.GetBytes(jwtKey);

        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id),
            new (ClaimTypes.Name, user.UserName!),
            new ("role", user.Role.ToLower())
        };

        var tokenDescriptor = new SecurityTokenDescriptor 
        { 
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
