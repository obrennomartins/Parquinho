using Stickers.Data;
using Stickers.Models.Dtos;
using Stickers.Models.Entities;
using Stickers.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Stickers.Services;

public class StickerService(StickersDbContext context, ILogger<StickerService> logger) : IStickerService
{
    public async Task<Sticker> CreateSticker(StickerCreateDto stickerDto)
    {
        logger.LogInformation("Processing sticker registration with number: {Numero}", stickerDto.Number);

        if (await StickerExists(stickerDto.Number))
        {
            logger.LogWarning("Attempt to register sticker with duplicate number: {Numero}", stickerDto.Number);
            throw new ConflictException($"A sticker with the number {stickerDto.Number} already exists.");
        }

        var sticker = new Sticker
        {
            Description = stickerDto.Description,
            Number = stickerDto.Number
        };

        context.Stickers.Add(sticker);
        await context.SaveChangesAsync();

        logger.LogInformation("Sticker successfully registered. ID: {Id}, Number: {Numero}", sticker.Id, sticker.Number);

        return sticker;
    }

    public async Task<IEnumerable<Sticker>> ListStickers()
    {
		logger.LogInformation("Processing listing of all stickers");

		var stickers = await context.Stickers
            .OrderBy(f => f.Number)
            .ToListAsync();

		logger.LogInformation("Returning {Count} stickers", stickers.Count);

        return stickers;
    }

    private async Task<bool> StickerExists(int number)
    {
        return await context.Stickers.AnyAsync(f => f.Number == number);
    }
}
