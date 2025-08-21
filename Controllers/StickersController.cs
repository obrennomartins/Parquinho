using Stickers.Models.Dtos;
using Stickers.Models.Entities;
using Stickers.Models.Exceptions;
using Stickers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stickers.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class StickersController(IStickerService stickerService, ILogger<StickersController> logger) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "collector")]
    public async Task<ActionResult<Sticker>> CreateSticker(StickerCreateDto stickerDto)
    {
        var username = User.FindFirst("username")?.Value;
        logger.LogInformation("Attempting to register sticker by user: {Username}, Number: {Number}", username, stickerDto.Number);

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid data for sticker registration. User: {Username}", username);
            return BadRequest(ModelState);
        }

        var sticker = await stickerService.CreateSticker(stickerDto);
        return CreatedAtAction(nameof(ListStickers), new { id = sticker.Id }, sticker);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Sticker>>> ListStickers()
    {
        var username = User.FindFirst("username")?.Value;
        logger.LogInformation("Sticker list requested by user: {Username}", username);

        var stickers = await stickerService.ListStickers();
        return Ok(stickers);
    }
}
