using HireMind.Domain.IRepositories;

namespace HireMind.Application.Commands.Authentication.Tokens;
public record LoginCommand(LoginRqDto Body) : IRequest<LoginResult>;
public record LoginResult(LoginRsDto Response);
public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Body.Email).NotEmpty().WithMessage("Email cannot be empty");

        RuleFor(x => x.Body.Password).NotEmpty().WithMessage("Password cannot be empty");
    }
}

public class LoginHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginHandler(
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetUserByEmailWithTokens(request.Body.Email, cancellationToken);

        if (user is null)
            return new LoginResult(new LoginRsDto("", "", false, "Invalid credentials"));

        if (!user.IsActive)
            return new LoginResult(new LoginRsDto("", "", false, "Account inactive"));

        if (user.EmailVerificationToken != null)
            return new LoginResult(new LoginRsDto("", "", false, "Email not verified"));

        if (user.IsLocked)
            return new LoginResult(new LoginRsDto("", "", false, "Account locked"));

        var passwordValid = _passwordHasher.Verify(
            request.Body.Password,
            user.PasswordHash);

        if (!passwordValid)
            return new LoginResult(new LoginRsDto("", "", false, "Invalid credentials"));

        //not allow multiple device login: Revoke all old refresh tokens
        //foreach (var t in user.RefreshTokens.Where(x => x.IsActive))
        //    t.Revoked = true;

        // Increase token version to invalidate old access tokens
        //user.IncrementTokenVersion();

        var accessToken = _tokenService.GenerateAccessToken(user);

        var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

        // Revoke all active refresh tokens for the same user and same device
        var tokensToRevoke = user.RefreshTokens.Where(x => x.Ip == ip && x.UserAgent == userAgent && x.Revoked == false).ToList();

        foreach (var token in tokensToRevoke)
        {
            token.Revoked = true;
            await _tokenRepository.UpdateToken(token, cancellationToken); 
        }
        var (plainRefreshToken, refreshTokenEntity) = _tokenService.GenerateRefreshToken(ip ?? "", userAgent ?? "");
        refreshTokenEntity.Id = Guid.NewGuid(); 
        refreshTokenEntity.UserId = user.Id;

        await _tokenRepository.AddToken(refreshTokenEntity, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        var response = new LoginRsDto(
            accessToken,
            plainRefreshToken,
            true,
            "Login successful"
        );

        return new LoginResult(response);
    }
}