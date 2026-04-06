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

    public LogoutHandler(
        ITokenRepository tokenRepository,
        IUnitOfWork unitOfWork)
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LogoutResult> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        using var sha = SHA256.Create();
        var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(request.Body.RefreshToken)));

        var existingToken = await _tokenRepository.GetByHash(hash, cancellationToken);

        existingToken!.Revoked = true;

        await _tokenRepository.UpdateToken(existingToken, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new LogoutResult(true, "Logout successful");
    }
}