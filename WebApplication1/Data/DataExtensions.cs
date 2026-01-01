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
                    new Genre { Name = "Action RPG" },
                    new Genre { Name = "Puzzle" }
                );
                dbContext.SaveChanges();
            }
        }
    }
    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("GameStore");

        //DbContext has a Scoped lifetime beacase:
// 1. It ensures that a new instance of DbContext is created per request
// 2. DB connections are a limited and expensive resources
// 3.  DbContext is not thread-safe. Scoped avoids to concurrency issues
// 4. Makes it easier to manage transactions and ensure data consistency across multiple operations within a single request
// 5. Reusing a Dbcontext instance can lead to increase memory consumption and potential memory leaks

        // builder.Services.AddSqlite<GameStoreContext>(connString);

        // Change AddSqlite to AddDbContext
        builder.Services.AddDbContext<GameStoreContext>(
            options => options.UseSqlite(connString));

    }


}
