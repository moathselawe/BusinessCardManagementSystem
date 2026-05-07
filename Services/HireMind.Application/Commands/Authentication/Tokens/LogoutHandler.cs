using Microsoft.AspNetCore.Http;

namespace HireMind.Application.Commands.Authentication.Tokens;
public record LogoutRqDto(string RefreshToken);
public record LogoutCommand(LogoutRqDto Body) : IRequest<LogoutResult>;
public record LogoutResult(bool IsSuccess, string Message);
public class LogoutValidator : AbstractValidator<LogoutCommand>
{
    public LogoutValidator()
    {
        RuleFor(x => x.Body.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required");
    }
}
public class LogoutHandler : IRequestHandler<LogoutCommand, LogoutResult>
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public LogoutHandler(
        ITokenRepository tokenRepository,
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository)
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;

        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<LogoutResult> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        using var sha = SHA256.Create();
        var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(request.Body.RefreshToken)));

        var existingToken = await _tokenRepository.GetByHash(hash, cancellationToken);

        if (existingToken is null)
            return new LogoutResult(false, "Invalid refresh token");

        var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

        // 🔥 revoke ALL tokens for same user + same device
        var tokensToRevoke = await _tokenRepository
            .GetActiveTokensByUserDevice(existingToken.UserId, ip ?? "", userAgent ?? "", cancellationToken);

        foreach (var token in tokensToRevoke)
        {
            token.Revoked = true;
            await _tokenRepository.UpdateToken(token, cancellationToken);
        }
        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new LogoutResult(true, "Logout successful");
    }
}