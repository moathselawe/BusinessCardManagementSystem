namespace HireMind.Application.Queries.ManageJobs;
public record GetJobByIdQuery(Guid Id) : IRequest<GetJobByIdResult>;
public record GetJobByIdResult(GetJobResponseDto response);
public class GetJobByIdHandlerValidator : AbstractValidator<GetJobByIdQuery>
{
    public GetJobByIdHandlerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
internal class GetJobByIdHandler : IRequestHandler<GetJobByIdQuery, GetJobByIdResult>
{
    private readonly IJobRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public GetJobByIdHandler(IJobRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<GetJobByIdResult> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            return new GetJobByIdResult(null!);

        var dto = GetJobResponseDto.FromEntity(entity);
        return new GetJobByIdResult(dto);
    }
}
