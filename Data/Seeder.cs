using Figurinhas.Models.Entities;
using Serilog;

namespace Figurinhas.Data;

public static class DataSeeder
{
    public static void SeedData(FigurinhasDbContext context)
    {
        if (!context.Usuarios.Any())
        {
            context.Usuarios.AddRange(
                new Usuario { Id = 1, Username = "colecionador1", Password = "123456", Role = "colecionador" },
                new Usuario { Id = 2, Username = "apreciador1", Password = "123456", Role = "apreciador" }
            );

            context.SaveChanges();
            Log.Information("Dados iniciais criados com sucesso");
        }
    }
}
