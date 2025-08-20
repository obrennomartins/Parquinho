using Figurinhas.Models.DTOs;
using Figurinhas.Services;
using Microsoft.AspNetCore.Mvc;

namespace Figurinhas.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            _logger.LogInformation("Tentativa de login para usuário: {Username}", loginDto.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados de login inválidos para usuário: {Username}", loginDto.Username);
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginDto);

            if (result == null)
            {
                return Unauthorized(new { message = "Credenciais inválidas" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno durante o login");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }
}
