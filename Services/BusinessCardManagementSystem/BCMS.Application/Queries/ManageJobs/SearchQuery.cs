using BCMS.Domain.Dtos.SharedDtos;

namespace BCMS.Application.Queries.ManageJobs;

public record SearchJobsQuery(SearchFiltersRqDto Filters) : IRequest<SearchFiltersRsDto<GetJobResponseDto>>;

internal class SearchHandler : IRequestHandler<SearchJobsQuery, SearchFiltersRsDto<GetJobResponseDto>>
{
    private readonly IJobRepository _repo;

    public SearchHandler(IJobRepository repo)
    {
        _repo = repo;
    }

    public async Task<SearchFiltersRsDto<GetJobResponseDto>> 
        Handle(SearchJobsQuery request, CancellationToken cancellationToken)
    {
        return await _repo.SearchAsync(request.Filters, cancellationToken);
    }
}
