namespace HireMind.Application.Commands.Shared;
public record DeleteLookupCommand(Guid Id) : IRequest<DeleteLookupResult>;
public record DeleteLookupResult(bool IsSuccess);
public class DeleteLookupHandlerValidator : AbstractValidator<DeleteLookupCommand>
{
    public DeleteLookupHandlerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
public class DeleteLookupHandler : IRequestHandler<DeleteLookupCommand, DeleteLookupResult>
{
    private readonly ILookupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLookupHandler(ILookupRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<DeleteLookupResult> Handle(DeleteLookupCommand request, CancellationToken cancellationToken)
    {
        var success = await _repository.DeleteAsync(request.Id, cancellationToken);

        if (success)
            await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new DeleteLookupResult(success);
    }
}

