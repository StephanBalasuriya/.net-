using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public record CreateGameDto(
    [Required] string Name, 
    string Genre, 
    decimal Price, 
    DateOnly ReleaseDate
    );
