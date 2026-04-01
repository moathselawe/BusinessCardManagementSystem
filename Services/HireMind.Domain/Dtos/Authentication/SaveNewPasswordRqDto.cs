namespace HireMind.Domain.Dtos.Authentication;
public record SaveNewPasswordRqDto(string Email, string Otp, string Password, string ConfirmPassword);
