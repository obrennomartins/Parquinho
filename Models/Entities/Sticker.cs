using System.ComponentModel.DataAnnotations;

namespace Stickers.Models.Entities;

public class Sticker
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
    [MaxLength(100, ErrorMessage = "The description must be a maximum of 100 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Number is required")]
    [Range(1, 50, ErrorMessage = "The number must be between 1 and 50")]
    public int Number { get; set; }
}