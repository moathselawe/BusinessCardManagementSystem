namespace BCMS.Domain.Dtos;

public record BusinessCardPreviewDto(
    string ArabicName,
    string EnglishName,
    DateTime? DateOfBirth,
    string Email,
    string Phone,
    string? Logo,
    string? Address
);


