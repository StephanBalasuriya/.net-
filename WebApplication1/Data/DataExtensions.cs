using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
         using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
            dbContext.Database.Migrate();

            // Seed genres
            if (!dbContext.Genres.Any())
            {
                dbContext.Genres.AddRange(
                    new Genre { Name = "Action-adventure" },
                    new Genre { Name = "Sandbox" },
                    new Genre { Name = "Action RPG" }
                );
                dbContext.SaveChanges();
            }
        }
    }
    
}
