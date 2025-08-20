using System.ComponentModel.DataAnnotations;

namespace Figurinhas.Models.Entities;

public class Figurinha
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [MaxLength(100, ErrorMessage = "A descrição deve ter no máximo 100 caracteres")]
    public string Descricao { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "O número é obrigatório")]
    [Range(1, 50, ErrorMessage = "O número deve estar entre 1 e 50")]
    public int Numero { get; set; }
    
    public DateTime DataCadastro { get; set; } = DateTime.Now;
}