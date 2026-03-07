namespace BCMS.Domain.Dtos.BusinessCard;

public record BusinessCardPreviewDto(
    string ArabicName,
    string EnglishName,
    DateTime? DateOfBirth,
    string Email,
    string Phone,
    string? Logo,
    string? Address
);


