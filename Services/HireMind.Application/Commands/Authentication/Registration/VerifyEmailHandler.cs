namespace HireMind.Application.Commands.Authentication.Registration;
public record VerifyEmailCommand(string PlainToken) : IRequest<VerifyEmailResult>;
public record VerifyEmailResult(string Message, bool IsSuccess, bool IsCanResendVerfication,string? email = null);

public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(x => x.PlainToken).NotEmpty().WithMessage("Invalid URL");

    }
}
internal class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, VerifyEmailResult>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyEmailHandler(IPasswordHasher passwordHasher, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<VerifyEmailResult> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var userFromDB = await _userRepository.GetUserByPlainToken(request.PlainToken, cancellationToken);

        if(userFromDB == null)
        {
            return new VerifyEmailResult("Invalid link or User already verified", false, false);
        }

        if (userFromDB.EmailVerificationTokenExpiresAt <= DateTime.UtcNow ||
            string.IsNullOrEmpty(userFromDB.EmailVerificationToken))
            return new VerifyEmailResult("Link expired, Email not verified", false, true, userFromDB.Email);

        var isVerifiedToken = _passwordHasher.Verify(request.PlainToken, userFromDB.EmailVerificationToken);
        
        if (!isVerifiedToken)
            return new VerifyEmailResult("Invalid or expired link", false, true, userFromDB.Email);

        userFromDB.UpdateVerifiedUser();

        var isUserVerified = await _userRepository.ModifyUser(userFromDB, cancellationToken);

        if (!isUserVerified)
            return new VerifyEmailResult("Error while verifying email", false, true,userFromDB.Email);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new VerifyEmailResult("Email verified successfully, Welcome To HireMind", true, false);
    }
}
