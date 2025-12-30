
using WebApplication1.Dtos;

namespace WebApplication1.Endpoints;

public static class GamesEndpoints
{

    private static readonly List<GameDto> games = new()
    {
    new GameDto(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
    new GameDto(2, "God of War", "Action-adventure", 49.99m, new DateOnly(2018, 4, 20)),
    new GameDto(3, "Red Dead Redemption 2", "Action-adventure", 39.99m, new DateOnly(2018, 10, 26)),
    new GameDto(4, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18)),
    new GameDto(5, "The Witcher 3: Wild Hunt", "Action RPG", 29.99m, new DateOnly(2015, 5, 19))
    };
    public static void MapGamesEndpoints(this WebApplication app)
    {
        //Get all games
        app.MapGet("/games", () => games);

        //get game by id
        app.MapGet("/games/{id}", (int id) =>
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        }).WithName("GetGameById");



        //post new game
        app.MapPost("/games", (CreateGameDto createGameDto) =>
        {
            var newGame = new GameDto
            (
                Id: games.Max(g => g.Id) + 1,
                Name: createGameDto.Name,
                Genre: createGameDto.Genre,
                Price: createGameDto.Price,
                ReleaseDate: createGameDto.ReleaseDate
            );

            games.Add(newGame);

            return Results.CreatedAtRoute(
                "GetGameById",
                new { id = newGame.Id },
                newGame
            );
        });


        //update existing game
        app.MapPut("/games/{id}", (int id, UpdateGameDto updateGameDto) =>
        {
            var existingGameIndex = games.FindIndex(g => g.Id == id);
            if (existingGameIndex == -1)
            {
                return Results.NotFound();
            }

            var updatedGame = new GameDto
            (
                Id: id,
                Name: updateGameDto.Name,
                Genre: updateGameDto.Genre,
                Price: updateGameDto.Price,
                ReleaseDate: updateGameDto.ReleaseDate
            );

            games[existingGameIndex] = updatedGame;

            return Results.NoContent();
        });

        //delete a game
        app.MapDelete("/games/{id}", (int id) =>
        {

            games.RemoveAll(games => games.Id == id);
            return Results.NoContent();
        });
    }


}
