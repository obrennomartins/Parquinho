using Stickers.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Stickers.Data;

public class StickersDbContext(DbContextOptions<StickersDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Sticker> Stickers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Sticker>()
            .HasIndex(f => f.Number)
            .IsUnique();
    }
}
