namespace HireMind.Domain.Dtos.BusinessCard;
public record UpdateBusinessCardDto(int Id, string ArabicName, string EnglishName, DateTime DateOfBirth, string Email,
    string Phone, string? Logo, string Address);
