namespace HireMind.Application.Commands.Security;
public record DeleteUserCommand(Guid Id) : IRequest<DeleteUserResult>;
public record DeleteUserResult(bool IsSuccess);
public class DeleteUserHandlerValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserHandlerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserResult>
{
    private readonly IUserRepository _UserRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserHandler(IUserRepository UserRepository, IUnitOfWork unitOfWork)
    {
        _UserRepository = UserRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<DeleteUserResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var success = await _UserRepository.DeleteAsync(request.Id, cancellationToken);

        if (success)
            await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new DeleteUserResult(success);
    }
}

