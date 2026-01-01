
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

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
        var group = app.MapGroup("/games");

        //Get all games
        group.MapGet("/", () => games);

        //get game by id
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        }).WithName("GetGameById");



        //post new game
        group.MapPost("/", (CreateGameDto createGameDto, GameStoreContext dbcontext) =>
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
            dbcontext.SaveChanges();

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
        group.MapPut("/{id}", (int id, UpdateGameDto updateGameDto) =>
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
        group.MapDelete("/{id}", (int id) =>
        {

            games.RemoveAll(games => games.Id == id);
            return Results.NoContent();
        });
    }


}
