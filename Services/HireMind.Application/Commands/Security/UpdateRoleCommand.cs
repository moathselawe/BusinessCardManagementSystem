namespace HireMind.Application.Commands.Security;

public record UpdateRoleCommand(UpdateRoleRqDto Body) : IRequest<UpdateRoleResult>;

public record UpdateRoleResult(bool IsSuccess);

public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleValidator()
    {
        RuleFor(x => x.Body.Id)
            .NotEmpty();

        RuleFor(x => x.Body.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.Body.Description)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);
    }
}
public class UpdateRoleHandler
    : IRequestHandler<UpdateRoleCommand, UpdateRoleResult>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateRoleResult> Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetById(request.Body.Id, cancellationToken);

        if (role == null)
            throw new Exception("Role not found");

        var roles = await _roleRepository.GetAllAsync(cancellationToken);

        var exists = roles.Any(r =>
            r.Id != request.Body.Id &&
            r.Name.ToLower() == request.Body.Name.Trim().ToLower());

        if (exists)
            throw new Exception("Role name already exists");

        role.Update(
            request.Body.Name.Trim(),
            request.Body.Description?.Trim(),
            request.Body.PermissionIds
        );

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new UpdateRoleResult(true);
    }
}