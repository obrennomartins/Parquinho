using Stickers.Models.Dtos;
using Stickers.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Stickers.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDto>> Register(RegisterDto registerDto)
    { 
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await authService.Register(registerDto);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        logger.LogInformation("Login attempt for user: {Username}", loginDto.Username);

        var result = await authService.Login(loginDto);

        if (result == null)
        {
            logger.LogWarning("Invalid credentials for user: {Username}", loginDto.Username);
            return Unauthorized(new { message = "Invalid credentials" });
        }

        return Ok(result);
    }
}
