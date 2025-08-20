using Figurinhas.Data;
using Figurinhas.Models.DTOs;
using Figurinhas.Models.Entities;
using Figurinhas.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Figurinhas.Services;

public class FigurinhaService : IFigurinhaService
{
    private readonly FigurinhasDbContext _context;
    private readonly ILogger<FigurinhaService> _logger;

    public FigurinhaService(FigurinhasDbContext context, ILogger<FigurinhaService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Figurinha> CadastrarFigurinhaAsync(FigurinhaCreateDto figurinhaDto)
    {
        _logger.LogInformation("Processando cadastro de figurinha com número: {Numero}", figurinhaDto.Numero);

        // Verifica se já existe uma figurinha com o mesmo número
        if (await FigurinhaExisteAsync(figurinhaDto.Numero))
        {
            _logger.LogWarning("Tentativa de cadastrar figurinha com número duplicado: {Numero}", figurinhaDto.Numero);
            throw new ConflictException($"Já existe uma figurinha cadastrada com o número {figurinhaDto.Numero}");
        }

        var figurinha = new Figurinha
        {
            Descricao = figurinhaDto.Descricao,
            Numero = figurinhaDto.Numero,
            DataCadastro = DateTime.Now
        };

        _context.Figurinhas.Add(figurinha);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Figurinha cadastrada com sucesso. ID: {Id}, Número: {Numero}",
            figurinha.Id, figurinha.Numero);

        return figurinha;
    }

    public async Task<IEnumerable<Figurinha>> ListarFigurinhasAsync()
    {
        _logger.LogInformation("Processando listagem de todas as figurinhas");

        var figurinhas = await _context.Figurinhas
            .OrderBy(f => f.Numero)
            .ToListAsync();

        _logger.LogInformation("Retornando {Count} figurinhas", figurinhas.Count);

        return figurinhas;
    }

    public async Task<bool> FigurinhaExisteAsync(int numero)
    {
        return await _context.Figurinhas.AnyAsync(f => f.Numero == numero);
    }
}
