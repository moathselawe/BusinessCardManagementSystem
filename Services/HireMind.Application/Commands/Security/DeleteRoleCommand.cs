namespace HireMind.Application.Commands.Security;
public record DeleteRoleCommand(Guid Id) : IRequest<DeleteRoleResult>;
public record DeleteRoleResult(bool IsSuccess);
public class DeleteRoleHandlerValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleHandlerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, DeleteRoleResult>
{
    private readonly IRoleRepository _RoleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleHandler(IRoleRepository RoleRepository, IUnitOfWork unitOfWork)
    {
        _RoleRepository = RoleRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<DeleteRoleResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var success = await _RoleRepository.DeleteAsync(request.Id, cancellationToken);

        if (success)
            await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new DeleteRoleResult(success);
    }
}

