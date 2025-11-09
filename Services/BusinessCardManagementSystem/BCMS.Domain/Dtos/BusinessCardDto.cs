namespace BCMS.Domain.Dtos;

public record BusinessCardDto(
    Guid Id,
    string ArabicName,
    string EnglishName,
    DateTime DateOfBirth,
    string Email,
    string Phone,
    string? Logo,
    string Address
    );
