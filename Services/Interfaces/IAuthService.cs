using Stickers.Models.Dtos;

namespace Stickers.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterResponseDto> Register(RegisterDto registerDto);
    Task<LoginResponseDto?> Login(LoginDto loginDto);
}
