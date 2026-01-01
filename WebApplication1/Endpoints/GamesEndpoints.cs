
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Endpoints;

public static class GamesEndpoints
{

    private static readonly List<GameSummaryDto> games = new()
    {
    new GameSummaryDto(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
    new GameSummaryDto(2, "God of War", "Action-adventure", 49.99m, new DateOnly(2018, 4, 20)),
    new GameSummaryDto(3, "Red Dead Redemption 2", "Action-adventure", 39.99m, new DateOnly(2018, 10, 26)),
    new GameSummaryDto(4, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18)),
    new GameSummaryDto(5, "The Witcher 3: Wild Hunt", "Action RPG", 29.99m, new DateOnly(2015, 5, 19))
    };
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        //Get all games
        group.MapGet("/", async (GameStoreContext dbContext)
            => await dbContext.Games
            .Include(g => g.Genre)
             .Select(game => new GameSummaryDto(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            ))
            .AsNoTracking()
            .ToListAsync());

        //get game by id
        group.MapGet("/{id}", async (int id, GameStoreContext dbcontext) =>
        {
            var game = await dbcontext.Games.FindAsync(id);
            return game is not null ? Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            ) : Results.NotFound();
        }).WithName("GetGameById");



        //post new game
        group.MapPost("/", async (CreateGameDto createGameDto, GameStoreContext dbcontext) =>
        {

            // // validating input
            // if (string.IsNullOrEmpty(createGameDto.Name))
            // {
            //     return Results.BadRequest("Game name cannot be empty.");
            // }//when using this method for validation, we have to type this for all inputs

            // var newGame = new GameDto
            // (
            //     Id: games.Max(g => g.Id) + 1,
            //     Name: createGameDto.Name,
            //     Genre: createGameDto.Genre,
            //     Price: createGameDto.Price,
            //     ReleaseDate: createGameDto.ReleaseDate
            // );

            // games.Add(newGame);

            // return Results.CreatedAtRoute(
            //     "GetGameById",
            //     new { id = newGame.Id },
            //     newGame
            // );

            Game newGame = new()
            {
                Name = createGameDto.Name,
                GenreId = createGameDto.GenreId,
                Price = createGameDto.Price,
                ReleaseDate = createGameDto.ReleaseDate
            };

            dbcontext.Games.Add(newGame);
            await dbcontext.SaveChangesAsync();

            GameDetailsDto gameDto = new(
                            newGame.Id,
                            newGame.Name,
                            newGame.GenreId,
                            newGame.Price,
                            newGame.ReleaseDate
                        );

            return Results.CreatedAtRoute(
                "GetGameById",
                new { id = newGame.Id },
                gameDto
            );



        });


        //update existing game
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGameDto, GameStoreContext dbContext) =>
        {
            // var existingGameIndex = games.FindIndex(g => g.Id == id);
            // if (existingGameIndex == -1)
            // {

            var existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }

            // var updatedGame = new GameSummaryDto
            // (
            //     Id: id,
            //     Name: updateGameDto.Name,
            //     Genre: updateGameDto.Genre,
            //     Price: updateGameDto.Price,
            //     ReleaseDate: updateGameDto.ReleaseDate
            // );

            // games[existingGameIndex] = updatedGame;
            existingGame.Name = updateGameDto.Name;
            existingGame.GenreId = updateGameDto.GenreID;
            existingGame.Price = updateGameDto.Price;
            existingGame.ReleaseDate = updateGameDto.ReleaseDate;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //delete a game
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {

            // var game = await dbContext.Games.FindAsync(id);
            // if (game is not null)
            // {
            //     dbContext.Games.Remove(game);
            //     await dbContext.SaveChangesAsync();
            // }
            // return Results.NoContent();


            await dbContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();


        });
    }


}
