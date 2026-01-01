namespace WebApplication1.Dtos;

//DTO is a contract between client and server since it represents 
//a shared agreement about how data will be transferred and used.

public record GameSummaryDto
(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);