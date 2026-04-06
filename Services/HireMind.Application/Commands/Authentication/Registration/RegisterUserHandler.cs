using HireMind.Domain.Dtos.Authentication;
using HireMind.Domain.Settings;
using Microsoft.Extensions.Options;

namespace HireMind.Application.Commands.Authentication.Registration;

public record RegisterUserCommand(RegisterUserRqDto Body) : IRequest<RegisterUserResult>;
public record RegisterUserResult(string Id);
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        // English Name
        RuleFor(x => x.Body.EnglishName)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(3)
            .WithMessage("Name must be at least 3 characters.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");

        // Arabic Name
        RuleFor(x => x.Body.ArabicName)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(3)
            .WithMessage("Name must be at least 3 characters.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");

        // Email
        RuleFor(x => x.Body.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .MaximumLength(150)
            .WithMessage("Email cannot exceed 150 characters.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        // Mobile
        RuleFor(x => x.Body.Mobile)
            .NotEmpty()
            .WithMessage("Mobile number is required.")
            .Matches(@"^[0-9]{8,15}$")
            .WithMessage("Mobile must contain only numbers (8–15 digits).");

        // Password
        RuleFor(x => x.Body.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters.")
            .MaximumLength(50)
            .WithMessage("Password cannot exceed 50 characters.")
            .Matches(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*(),.?""{}|<>]).{8,50}$")
            .WithMessage("Password must contain at least 1 uppercase letter, 1 number, and 1 symbol.");

        // Confirm Password
        RuleFor(x => x.Body.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm password is required.");

        // Password Match
        RuleFor(x => x.Body)
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage("Password and Confirm Password must match.");
    }
}
internal class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly INotificationService _notificationService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EmailVerificationSettings _emailVerificationSettings;

    public RegisterUserHandler(
        IPasswordHasher passwordHasher,
        INotificationService notificationService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IOptions<EmailVerificationSettings> emailVerificationSettings)
    {
        _passwordHasher = passwordHasher;
        _notificationService = notificationService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _emailVerificationSettings = emailVerificationSettings.Value;
    }
    public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
       try
        {
            var exists = await _userRepository.EmailExists(request.Body.Email, cancellationToken);
            if (exists)
                throw new Exception("Email already registered");

            var hashedPassword = _passwordHasher.Hash(request.Body.Password);

            var plainToken = Guid.NewGuid().ToString();
            var emailVerificationTokenHash = _passwordHasher.Hash(plainToken);

            var emailVerificationTokenExpiresAt = DateTime.UtcNow.AddMinutes(_emailVerificationSettings.TokenExpirationMinutes);

            var user = UserModel.RegisterUser(
                request.Body.EnglishName,
                request.Body.ArabicName,
                request.Body.Mobile,
                request.Body.Email,
                hashedPassword,
                emailVerificationTokenHash,
                emailVerificationTokenExpiresAt
            );

            var userId = await _userRepository.CreateUser(user, cancellationToken);

            var subject = "VerficationEmail";

            var verfiyLink = _emailVerificationSettings.VerifyLink;

            var body = $"<p>Welcome to HireMind!</p>" +
                       $"<p>Please verify your registration by clicking the link below:</p>" +
                       $"<a href='{verfiyLink}/{plainToken}'>Verify Email</a>";

            await _notificationService.SendEmailAsync(request.Body.Email, subject, body);

            await _unitOfWork.SaveWorkAsync(cancellationToken);

            return new RegisterUserResult(userId.ToString());
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

