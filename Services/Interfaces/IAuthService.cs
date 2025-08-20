using Figurinhas.Models.DTOs;
using Figurinhas.Models.Entities;

namespace Figurinhas.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    string GenerateJwtToken(Usuario usuario);
}
