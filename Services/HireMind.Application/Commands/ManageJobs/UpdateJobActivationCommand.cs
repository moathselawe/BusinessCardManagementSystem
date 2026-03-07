namespace HireMind.Application.Commands.ManageJobs;

public record UpdateJobActivationCommand(UpdateJobActivationRequestDto request) : IRequest<UpdateJobActivationResult>;
public record UpdateJobActivationResult(bool IsSuccess);

public class UpdateJobActivationValidator : AbstractValidator<UpdateJobActivationCommand>
{
    public UpdateJobActivationValidator()
    {

    }

}
public class UpdateJobActivationHandler : IRequestHandler<UpdateJobActivationCommand, UpdateJobActivationResult>
{
    private readonly IJobRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateJobActivationHandler(IJobRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<UpdateJobActivationResult> Handle(UpdateJobActivationCommand command, CancellationToken cancellationToken)
    {
        var job = jobModel.UpdateActivation(
            id: command.request.Id,
            isActive: command.request.IsActive
        );


        var result = await _repository.UpdateAsync(job, true, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new UpdateJobActivationResult(result);
    }
}

