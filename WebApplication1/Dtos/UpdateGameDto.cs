namespace WebApplication1.Dtos;

public record UpdateGameDto(
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);