namespace HireMind.Application.Commands.Security;

public record UpdateUserCommand(UpdateUserRequestDto request) : IRequest<UpdateUserResult>;
public record UpdateUserResult(bool IsSuccess);

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {

    }

}
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserHandler(IUserRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserById(command.request.Id, cancellationToken);

        if (user == null)
            return new UpdateUserResult(false);

        
        var LoginAttempts = user.IsLocked != command.request.IsLocked ? 0 : user.FailedLoginAttempts;
        var lockDate = user.IsLocked != command.request.IsLocked ? DateTime.Now : user.LockedDate.GetValueOrDefault();

        user.Update(
        command.request.NameArabic,
        command.request.NameEnglish,
        command.request.Mobile,
        command.request.Address,
        command.request.Email,
        command.request.Gender,
        command.request.IsLocked,
        LoginAttempts,
        command.request.IsLocked ? lockDate : null );

        user.ClearRoles();

        foreach (var roleId in command.request.RoleIds)
        {
            user.AddRole(roleId);
        }

        //var result = await _repository.ModifyUser(user, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new UpdateUserResult(true);
    }
}

