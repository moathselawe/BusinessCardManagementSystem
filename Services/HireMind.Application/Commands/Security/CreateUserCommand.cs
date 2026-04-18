using HireMind.Domain.Enum;

namespace HireMind.Application.Commands.Security;
public record CreateUserByAdminCommand(CreateUserByAdminRqDto Body) : IRequest<CreateUserByAdminResult>;
public record CreateUserByAdminResult(string UserId);

public class CreateUserByAdminValidator : AbstractValidator<CreateUserByAdminCommand>
{
    public CreateUserByAdminValidator()
    {
        RuleFor(x => x.Body.NameEnglish)
            .NotEmpty().MinimumLength(3).MaximumLength(100);

        RuleFor(x => x.Body.NameArabic)
            .NotEmpty().MinimumLength(3).MaximumLength(100);

        RuleFor(x => x.Body.Email)
            .NotEmpty().EmailAddress().MaximumLength(150);

        RuleFor(x => x.Body.Mobile)
            .NotEmpty().Matches(@"^[0-9]{8,15}$");

        RuleFor(x => x.Body.RoleIds)
            .NotEmpty();
    }
}

public class CreateUserByAdminHandler
    : IRequestHandler<CreateUserByAdminCommand, CreateUserByAdminResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EmailVerificationSettings _emailSettings;

    public CreateUserByAdminHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        INotificationService notificationService,
        IUnitOfWork unitOfWork,
        IOptions<EmailVerificationSettings> emailSettings)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
        _emailSettings = emailSettings.Value;
    }

    public async Task<CreateUserByAdminResult> Handle(
        CreateUserByAdminCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _userRepository.EmailExists(request.Body.Email, cancellationToken);
        if (exists)
            throw new Exception("Email already exists");

        //var role = await _roleRepository.GetById(request.Body.RoleId, cancellationToken);
        //if (role == null)
        //    throw new Exception("Role not found");

        // 🔥 generate password setup token
        var plainToken = Guid.NewGuid().ToString();
        var hashedPassword = _passwordHasher.Hash(plainToken);


        // ❗ user created WITHOUT password (or you can set random hash)
        var user = UserModel.CreateByAdmin(
          request.Body.NameEnglish,
          request.Body.NameArabic,
          request.Body.Mobile,
          request.Body.Email,
          hashedPassword
          );

        foreach (var roleId in request.Body.RoleIds)
        {
            user.AddRole(roleId);
        }

        var userId = await _userRepository.CreateUser(user, cancellationToken);


        var subject = "HireMind Reset Password Manditory";

        var body =
            $"<p>Your account has been created by admin.</p>" +
            $"<p>Please reset your password to allow login</p>";

        await _notificationService.SendEmailAsync(user.Email, subject, body);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new CreateUserByAdminResult(userId.ToString());
    }
}
