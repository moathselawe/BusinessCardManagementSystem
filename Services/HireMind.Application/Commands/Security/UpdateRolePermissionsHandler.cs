namespace HireMind.Application.Commands.Security;

public record UpdateRolePermissionsCommand(UpdateRolePermissionsRqDto Body) : IRequest<UpdateRolePermissionsResult>;

public record UpdateRolePermissionsResult(bool IsSuccess);
public class UpdateRolePermissionsHandler : IRequestHandler<UpdateRolePermissionsCommand, UpdateRolePermissionsResult>
{
    private readonly IRoleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRolePermissionsHandler(IRoleRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateRolePermissionsResult> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await _repository.GetById(request.Body.RoleId, cancellationToken);

        if (role == null)
            throw new Exception("Role not found");

        var permissionIds = request.Body.PermissionIds ?? new List<Guid>();

        role.UpdatePermissions(permissionIds);

        _repository.Update(role);
        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new UpdateRolePermissionsResult(true);
    }
}