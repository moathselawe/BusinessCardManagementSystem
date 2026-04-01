using HireMind.Domain.Settings;
using Microsoft.Extensions.Options;

namespace HireMind.Application.Commands.Authentication.Registration;
public record ResendVerificationEmailRequest(string Email);
public record ResendVerificationEmailCommand(string Email) : IRequest<ResendVerificationEmailResult>;
public record ResendVerificationEmailResult(
    string Message,
    bool IsSuccess
);

public class ResendVerificationEmailHandler
    : IRequestHandler<ResendVerificationEmailCommand, ResendVerificationEmailResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EmailVerificationSettings _emailVerificationSettings;

    public ResendVerificationEmailHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        INotificationService notificationService,
        IUnitOfWork unitOfWork,
        IOptions<EmailVerificationSettings> emailVerificationSettings)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
        _emailVerificationSettings = emailVerificationSettings.Value;
    }

    public async Task<ResendVerificationEmailResult> Handle(
        ResendVerificationEmailCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Email, cancellationToken);

        if (user == null)
            return new ResendVerificationEmailResult("User not found", false);

        if (user.IsActive)
            return new ResendVerificationEmailResult("Email already verified", false);

        // generate new token
        var plainToken = Guid.NewGuid().ToString();
        var tokenHash = _passwordHasher.Hash(plainToken);

        var expiresAt = DateTime.UtcNow.AddMinutes(_emailVerificationSettings.TokenExpirationMinutes);

        user.UpdateUserEmailReVerification(tokenHash, expiresAt);

        var verifyLink = _emailVerificationSettings.VerifyLink;

        var subject = "Verify your email";

        var body =
            $"<p>Welcome to HireMind!</p>" +
            $"<p>Please re verify your email:</p>" +
            $"<a href='{verifyLink}/{plainToken}'>Verify Email</a>";

        await _notificationService.SendEmailAsync(user.Email, subject, body);

        await _userRepository.ModifyUser(user, cancellationToken);
        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new ResendVerificationEmailResult(
            "Verification email sent again",
            true
        );
    }
}