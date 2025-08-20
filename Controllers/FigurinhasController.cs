using Figurinhas.Models.DTOs;
using Figurinhas.Models.Entities;
using Figurinhas.Models.Exceptions;
using Figurinhas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Figurinhas.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class FigurinhasController : ControllerBase
{
    private readonly IFigurinhaService _figurinhaService;
    private readonly ILogger<FigurinhasController> _logger;

    public FigurinhasController(IFigurinhaService figurinhaService, ILogger<FigurinhasController> logger)
    {
        _figurinhaService = figurinhaService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Policy = "colecionador")]
    public async Task<ActionResult<Figurinha>> CadastrarFigurinha(FigurinhaCreateDto figurinhaDto)
    {
        try
        {
            var username = User.FindFirst("username")?.Value;
            _logger.LogInformation("Tentativa de cadastro de figurinha pelo usuário: {Username}, Número: {Numero}",
                username, figurinhaDto.Numero);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos para cadastro de figurinha. Usuário: {Username}", username);
                return BadRequest(ModelState);
            }

            var figurinha = await _figurinhaService.CadastrarFigurinhaAsync(figurinhaDto);
            return CreatedAtAction(nameof(ListarFigurinhas), new { id = figurinha.Id }, figurinha);
        }
        catch (ConflictException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno durante cadastro de figurinha");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Figurinha>>> ListarFigurinhas()
    {
        try
        {
            var username = User.FindFirst("username")?.Value;
            _logger.LogInformation("Listagem de figurinhas solicitada pelo usuário: {Username}", username);

            var figurinhas = await _figurinhaService.ListarFigurinhasAsync();
            return Ok(figurinhas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno durante listagem de figurinhas");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }
}
