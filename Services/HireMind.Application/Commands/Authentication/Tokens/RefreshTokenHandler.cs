namespace HireMind.Application.Commands.Authentication.Tokens;
public record RefreshTokenRqDto(string RefreshToken);
public record RefreshTokenCommand(RefreshTokenRqDto Body) : IRequest<RefreshTokenResult>;
public record RefreshTokenResult(LoginRsDto Response);

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.Body.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required");
    }
}

public class RefreshTokenHandler
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefreshTokenHandler(
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<RefreshTokenResult> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        using var sha = SHA256.Create();
        var hash = Convert.ToBase64String(
            sha.ComputeHash(Encoding.UTF8.GetBytes(request.Body.RefreshToken)));

        var existingToken = await _tokenRepository
            .GetByHash(hash, cancellationToken);

        if (existingToken is null)
            return new RefreshTokenResult(
                new LoginRsDto("", "", false, "Invalid refresh token"));

        if (!existingToken.IsActive)
            return new RefreshTokenResult(
                new LoginRsDto("", "", false, "Refresh token expired"));

        var user = await _userRepository
            .GetUserById(existingToken.UserId, cancellationToken);

        if (user == null || user.IsActive == false) 
            return new RefreshTokenResult(
                new LoginRsDto("", "", false, "Invalid User"));

        if (user is null)
            return new RefreshTokenResult(
                new LoginRsDto("", "", false, "User not found"));

        var ip = _httpContextAccessor.HttpContext?
            .Connection?.RemoteIpAddress?.ToString();

        var userAgent = _httpContextAccessor.HttpContext?
            .Request.Headers["User-Agent"].ToString();

        // Optional: enforce same device for refresh
        if (existingToken.Ip != ip || existingToken.UserAgent != userAgent)
        {
            return new RefreshTokenResult(
                new LoginRsDto("", "", false, "Invalid process"));
        }

        // not allow multiple device login: Check if token is used from same device/IP
        //if (existingToken.Ip != ip || existingToken.UserAgent != userAgent)
        //    return new RefreshTokenResult(new LoginRsDto("", "", false, "Token used from unknown device"));

        // Revoke old token
        existingToken.Revoked = true;

        var (plainRefreshToken, refreshTokenEntity) =
            _tokenService.GenerateRefreshToken(ip ?? "", userAgent ?? "");

        refreshTokenEntity.UserId = user.Id;

        await _tokenRepository.AddToken(refreshTokenEntity, cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(user);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new RefreshTokenResult(
            new LoginRsDto(
                accessToken,
                plainRefreshToken,
                true,
                "Token refreshed successfully"));
    }
}