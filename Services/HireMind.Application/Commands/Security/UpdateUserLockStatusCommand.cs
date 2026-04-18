namespace HireMind.Application.Commands.Security;

public record UpdateUserLockStatusCommand(UpdateUserLockStatusRequestDto request) : IRequest<UpdateUserLockStatusResult>;
public record UpdateUserLockStatusResult(bool IsSuccess);

public class UpdateUserLockStatusValidator : AbstractValidator<UpdateUserLockStatusCommand>
{
    public UpdateUserLockStatusValidator()
    {

    }

}
public class UpdateUserLockStatusHandler : IRequestHandler<UpdateUserLockStatusCommand, UpdateUserLockStatusResult>
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserLockStatusHandler(IUserRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<UpdateUserLockStatusResult> Handle(UpdateUserLockStatusCommand command, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserById(command.request.Id, cancellationToken);

        if (user == null)
            return new UpdateUserLockStatusResult(false);

        user.UpdateLockStatus(user.Id, user.IsLocked ? false : true);

        var result = await _repository.ModifyUser(user, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new UpdateUserLockStatusResult(result);
    }
}

