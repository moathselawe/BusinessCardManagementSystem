namespace HireMind.Application.Queries.Seurity;

public record SearchUsersQuery(SearchFiltersRqDto Filters) : IRequest<SearchFiltersRsDto<GetUserResponseDto>>;

internal class SearchHandler : IRequestHandler<SearchUsersQuery, SearchFiltersRsDto<GetUserResponseDto>>
{
    private readonly IUserRepository _repo;

    public SearchHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<SearchFiltersRsDto<GetUserResponseDto>> 
        Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        return await _repo.SearchAsync(request.Filters, cancellationToken);
    }
}
