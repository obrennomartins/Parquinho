using System.ComponentModel.DataAnnotations;

namespace Stickers.Models.Dtos;

public class StickerCreateDto
{
    [Required(ErrorMessage = "Description is required")]
    [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Number is required")]
    [Range(1, 50, ErrorMessage = "Number must be between 1 and 50")]
    public int Number { get; set; }
}
