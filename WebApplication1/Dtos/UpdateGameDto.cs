using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public record UpdateGameDto(
    [Required][StringLength(100)] string Name,
    [Range(1,50)] int GenreID,
    [Range(1,50)] decimal Price,
    DateOnly ReleaseDate
);