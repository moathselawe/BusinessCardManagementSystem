namespace HireMind.Application.Commands.Authentication.ResetPassword;
public record SendResetCodeRequest(string Email);
public record SendResetCodeCommand(string Email) : IRequest<SendResetCodeResult>;
public record SendResetCodeResult(string Message, bool IsSuccess);
public class SendResetCodeHandler : IRequestHandler<SendResetCodeCommand, SendResetCodeResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;

    public SendResetCodeHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        INotificationService notificationService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<SendResetCodeResult> Handle(SendResetCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Email, cancellationToken);

        if (user == null)
            return new SendResetCodeResult("User not found", false);

        var otp = new Random().Next(100000, 999999).ToString();

        var hashedOtp = _passwordHasher.Hash(otp);

        user.UpdatePasswordResetOtp(hashedOtp, DateTime.UtcNow.AddMinutes(10));

        await _userRepository.ModifyUser(user, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        var subject = "Reset Password Code";

        var body = $"<p>Your password reset code is:</p><h2>{otp}</h2>";

        await _notificationService.SendEmailAsync(user.Email, subject, body);

        return new SendResetCodeResult("Verification code sent", true);
    }
}