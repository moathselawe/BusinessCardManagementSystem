namespace HireMind.Application.Commands.ManageJobs;
public record DeleteJobCommand(Guid Id) : IRequest<DeleteJobResult>;
public record DeleteJobResult(bool IsSuccess);
public class DeleteJobHandlerValidator : AbstractValidator<DeleteJobCommand>
{
    public DeleteJobHandlerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
public class DeleteJobHandler : IRequestHandler<DeleteJobCommand, DeleteJobResult>
{
    private readonly IJobRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteJobHandler(IJobRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<DeleteJobResult> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
    {
        var success = await _repository.DeleteAsync(request.Id, cancellationToken);

        if (success)
            await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new DeleteJobResult(success);
    }
}

