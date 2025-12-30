using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public record CreateGameDto(
    [Required] [StringLength(40)]string Name, 
    string Genre, 
    [Range(0, 50)]decimal Price, 
    DateOnly ReleaseDate
    );
