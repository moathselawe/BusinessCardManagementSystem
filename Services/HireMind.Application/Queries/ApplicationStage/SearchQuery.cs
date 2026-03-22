using HireMind.Domain.Dtos.ApplicationStage;

namespace HireMind.Application.Queries.ApplicationStage;

public record SearchJobApplicationQuery(SearchJobApplicationsRequestDto Filters) : IRequest<List<JobApplicationDto>>;

internal class SearchHandler : IRequestHandler<SearchJobApplicationQuery, List<JobApplicationDto>>
{
    private readonly IApplicationStageRepository _repo;

    public SearchHandler(IApplicationStageRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<JobApplicationDto>> Handle(SearchJobApplicationQuery request, CancellationToken cancellationToken)
    {
        return await _repo.SearchJobApplicationsAsync(request.Filters, cancellationToken);
    }
}
