namespace HireMind.Domain.Dtos.Authentication;

public record LoginRsDto(
    string AccessToken,
    string RefreshToken,
    bool IsSuccess,
    string Message
);