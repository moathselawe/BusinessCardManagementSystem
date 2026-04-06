namespace HireMind.Application.Commands.Authentication.Tokens;
public record LogoutFromAllDevicesRqDto(string Email);
public record LogoutFromAllDevicesCommand(LogoutFromAllDevicesRqDto Body) : IRequest<LogoutFromAllDevicesResult>;
public record LogoutFromAllDevicesResult(bool IsSuccess, string Message);
public class LogoutFromAllDevicesValidator : AbstractValidator<LogoutFromAllDevicesCommand>
{
    public LogoutFromAllDevicesValidator()
    {
        RuleFor(x => x.Body.Email)
            .NotEmpty()
            .WithMessage("Email is required");
    }
}
public class LogoutFromAllDevicesHandler : IRequestHandler<LogoutFromAllDevicesCommand, LogoutFromAllDevicesResult>
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutFromAllDevicesHandler(
        ITokenRepository tokenRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LogoutFromAllDevicesResult> Handle(LogoutFromAllDevicesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailWithTokens(request.Body.Email, cancellationToken);

        if (user is null)
            return new LogoutFromAllDevicesResult(false, "Invalid credentials");

        var tokensToRevoke = user.RefreshTokens.Where(x => x.Revoked == false).ToList();

        foreach (var token in tokensToRevoke)
        {
            token.Revoked = true;
            await _tokenRepository.UpdateToken(token, cancellationToken);
        }

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new LogoutFromAllDevicesResult(true, "Logout All successful");
    }
}