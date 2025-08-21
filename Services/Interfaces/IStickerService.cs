using Stickers.Models.Dtos;
using Stickers.Models.Entities;

namespace Stickers.Services;

public interface IStickerService
{
    Task<Sticker> CreateSticker(StickerCreateDto stickerDto);
    Task<IEnumerable<Sticker>> ListStickers();
}
