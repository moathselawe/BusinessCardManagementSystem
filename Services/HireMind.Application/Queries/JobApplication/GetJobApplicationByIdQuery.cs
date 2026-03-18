using HireMind.Application.Queries.HiringStages;

namespace HireMind.Application.Queries.JobApplication;
public record GetJobApplicationByIdQuery(int Id) : IRequest<GetJobApplicationByIdResult>;
public record GetJobApplicationByIdResult(GetJobApplicationByIdDto Response);

internal class GetJobApplicationByIdHandler : IRequestHandler<GetJobApplicationByIdQuery, GetJobApplicationByIdResult>
{
    private readonly IJobApplicationRepository _repository;

    public GetJobApplicationByIdHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetJobApplicationByIdResult> Handle(GetJobApplicationByIdQuery request, CancellationToken cancellationToken)
    {
        var application = await _repository.GetByIdAsync(request.Id, cancellationToken);

        var dtos = application.Adapt<GetJobApplicationByIdDto>();

        return new GetJobApplicationByIdResult(dtos);
    }
}