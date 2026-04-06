namespace HireMind.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    (string PlainToken, RefreshToken Entity) GenerateRefreshToken(string ip, string userAgent);
}
