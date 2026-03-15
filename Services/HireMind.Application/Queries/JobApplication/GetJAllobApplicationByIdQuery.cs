namespace HireMind.Application.Queries.JobApplication;
public record GetAllJobApplicationsByJobIdQuery(int JobId) : IRequest<GetAllJobApplicationsByJobIdResult>;
public record GetAllJobApplicationsByJobIdResult(List<JobApplicationDto> Response);

internal class GetAllJobApplicationsByJobIdHandler: IRequestHandler<GetAllJobApplicationsByJobIdQuery, GetAllJobApplicationsByJobIdResult>
{
    private readonly IJobApplicationRepository _repository;

    public GetAllJobApplicationsByJobIdHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetAllJobApplicationsByJobIdResult> Handle(GetAllJobApplicationsByJobIdQuery request, CancellationToken cancellationToken)
    {
        var dtos = await _repository.GetAllJobApplicationsByJobIdAsync(request.JobId, cancellationToken);
        return new GetAllJobApplicationsByJobIdResult(dtos);
    }
}