namespace HireMind.Domain.Dtos.Authentication;
public record RegisterUserRqDto(
    string EnglishName,
    string ArabicName,
    string Mobile,
    string Email,
    string Password,
    string ConfirmPassword
);