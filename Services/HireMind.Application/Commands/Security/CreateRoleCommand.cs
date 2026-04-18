using DocumentFormat.OpenXml.Wordprocessing;
using HireMind.Domain.Enum;

namespace HireMind.Application.Commands.Security;
public record CreateRoleCommand(RoleRqDto Body) : IRequest<CreateRoleResult>;
public record CreateRoleResult(string RoleId);

public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.Body.Name)
            .NotEmpty().MinimumLength(3).MaximumLength(100);

        RuleFor(x => x.Body.Description)
            .NotEmpty().MinimumLength(3).MaximumLength(100);
    }
}

public class CreateRoleHandler
    : IRequestHandler<CreateRoleCommand, CreateRoleResult>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateRoleResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync(cancellationToken);

        var exists = roles.Any(r =>
            r.Name.ToLower() == request.Body.Name.Trim().ToLower());

        if (exists)
            throw new Exception("Role already exists");

        var role = Role.Create(
            request.Body.Name.Trim(),
            request.Body.Description?.Trim()
        );

        role.AddPermissions(request.Body.PermissionIds);

        var roleId = await _roleRepository.Create(role, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new CreateRoleResult(roleId.ToString());
    }
}
