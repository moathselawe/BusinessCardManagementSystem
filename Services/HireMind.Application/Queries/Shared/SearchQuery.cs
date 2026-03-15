namespace HireMind.Application.Queries.Shared;

public record SearchLookupsQuery(SearchFiltersRqDto Filters) : IRequest<SearchFiltersRsDto<GetLookupDto>>;

internal class SearchHandler : IRequestHandler<SearchLookupsQuery, SearchFiltersRsDto<GetLookupDto>>
{
    private readonly ILookupRepository _repo;

    public SearchHandler(ILookupRepository repo)
    {
        _repo = repo;
    }

    public async Task<SearchFiltersRsDto<GetLookupDto>> 
        Handle(SearchLookupsQuery request, CancellationToken cancellationToken)
    {
        return await _repo.SearchAsync(request.Filters, cancellationToken);
    }
}
