using Figurinhas.Models.DTOs;
using Figurinhas.Models.Entities;

namespace Figurinhas.Services;
public interface IFigurinhaService
{
    Task<Figurinha> CadastrarFigurinhaAsync(FigurinhaCreateDto figurinhaDto);
    Task<IEnumerable<Figurinha>> ListarFigurinhasAsync();
    Task<bool> FigurinhaExisteAsync(int numero);
}
