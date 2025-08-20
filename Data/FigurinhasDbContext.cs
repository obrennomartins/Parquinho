using Figurinhas.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Figurinhas.Data;

public class FigurinhasDbContext(DbContextOptions<FigurinhasDbContext> options) : DbContext(options)
{
    public DbSet<Figurinha> Figurinhas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO Mover isto para um Configuration próprio
        modelBuilder.Entity<Figurinha>()
            .HasIndex(f => f.Numero)
            .IsUnique();
    }
}