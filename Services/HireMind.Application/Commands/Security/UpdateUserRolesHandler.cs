namespace HireMind.Application.Commands.Security;

public record UpdateUserRolesCommand(UpdateUserRolesRqDto request) : IRequest<UpdateUserRolesResult>;

public record UpdateUserRolesResult(bool IsSuccess);
public class UpdateUserRolesHandler : IRequestHandler<UpdateUserRolesCommand, UpdateUserRolesResult>
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserRolesHandler(IUserRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateUserRolesResult> Handle(UpdateUserRolesCommand command, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserById(command.request.UserId, cancellationToken);

        if (user == null)
            return new UpdateUserRolesResult(false);

        user.ClearUserRoles();

        foreach (var roleId in command.request.RoleIds)
        {
            user.AddRole(roleId);
        }

        await _unitOfWork.SaveWorkAsync(cancellationToken);
        return new UpdateUserRolesResult(true);
    }
}