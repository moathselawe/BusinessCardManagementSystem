namespace HireMind.Application.Commands.Authentication.ResetPassword;
public record SaveNewPasswordCommand(SaveNewPasswordRqDto Request) : IRequest<SaveNewPasswordResult>;
public record SaveNewPasswordResult(string Message, bool IsSuccess);
public class SaveNewPasswordHandler
    : IRequestHandler<SaveNewPasswordCommand, SaveNewPasswordResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public SaveNewPasswordHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<SaveNewPasswordResult> Handle(SaveNewPasswordCommand command,CancellationToken cancellationToken)
    {
        if (command.Request.Password != command.Request.ConfirmPassword)
            return new SaveNewPasswordResult("Passwords do not match", false);

        var user = await _userRepository.GetUserByEmail(command.Request.Email, cancellationToken);

        if (user == null)
            return new SaveNewPasswordResult("User not found", false);

        var isValidOtp = _passwordHasher.Verify(command.Request.Otp, user.PasswordResetOtp);

        if (!isValidOtp)
            return new SaveNewPasswordResult("Invalid OTP", false);

        var hashedPassword = _passwordHasher.Hash(command.Request.Password);

        user.UpdateUserPassword(hashedPassword);

        await _userRepository.ModifyUser(user, cancellationToken);
        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new SaveNewPasswordResult("Password reset successful", true);
    }
}
