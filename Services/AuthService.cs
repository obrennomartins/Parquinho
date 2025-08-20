using Figurinhas.Data;
using Figurinhas.Models.DTOs;
using Figurinhas.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Figurinhas.Services;

public class AuthService : IAuthService
{
    private readonly FigurinhasDbContext _context;
    private readonly ILogger<AuthService> _logger;

    public AuthService(FigurinhasDbContext context, ILogger<AuthService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        _logger.LogInformation("Processando login para usuário: {Username}", loginDto.Username);

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.Password == loginDto.Password);

        if (usuario == null)
        {
            _logger.LogWarning("Credenciais inválidas para usuário: {Username}", loginDto.Username);
            return null;
        }

        var token = GenerateJwtToken(usuario);

        _logger.LogInformation("Login realizado com sucesso para usuário: {Username}, Role: {Role}",
            usuario.Username, usuario.Role);

        return new LoginResponseDto
        {
            Token = token,
            Role = usuario.Role,
            Username = usuario.Username
        };
    }

    public string GenerateJwtToken(Usuario usuario)
    {
        var key = Encoding.ASCII.GetBytes("chaveDasFigurinhasQueEhUltraSecreta");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim("id", usuario.Id.ToString()),
                    new Claim("username", usuario.Username),
                    new Claim("role", usuario.Role.ToLower())
                }),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
