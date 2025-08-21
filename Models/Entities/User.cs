using Microsoft.AspNetCore.Identity;

namespace Stickers.Models.Entities;

public class User : IdentityUser
{
    public string Role { get; set; } = string.Empty;
}
