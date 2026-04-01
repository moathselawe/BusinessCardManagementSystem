namespace HireMind.Application.Commands.Authentication.ResetPassword;
public record VerifyResetCodeRequest(string Email, string Otp);
public record VerifyResetCodeCommand(string Email, string Otp) : IRequest<VerifyResetCodeResult>;
public record VerifyResetCodeResult(string Message, bool IsSuccess);
public class VerifyResetCodeHandler : IRequestHandler<VerifyResetCodeCommand, VerifyResetCodeResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public VerifyResetCodeHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<VerifyResetCodeResult> Handle(VerifyResetCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Email, cancellationToken);

        if (user == null)
            return new VerifyResetCodeResult("User not found", false);

        if (user.PasswordResetOtpExpiresAt < DateTime.UtcNow)
            return new VerifyResetCodeResult("OTP expired", false);

        var isValidOtp = _passwordHasher.Verify(request.Otp, user.PasswordResetOtp);

        if (!isValidOtp)
            return new VerifyResetCodeResult("Invalid OTP", false);

        return new VerifyResetCodeResult("OTP verified", true);
    }
}
