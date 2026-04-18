namespace HireMind.Application.Queries.Seurity;
public record SearchRolesQuery(SearchFiltersRqDto Filters) : IRequest<SearchFiltersRsDto<GetRoleResponseDto>>;

internal class SearchRolesHandler : IRequestHandler<SearchRolesQuery, SearchFiltersRsDto<GetRoleResponseDto>>
{
    private readonly IRoleRepository _repo;

    public SearchRolesHandler(IRoleRepository repo)
    {
        _repo = repo;
    }

    public async Task<SearchFiltersRsDto<GetRoleResponseDto>> Handle(SearchRolesQuery request, CancellationToken cancellationToken)
    {
        return await _repo.SearchAsync(request.Filters, cancellationToken);
    }
}
