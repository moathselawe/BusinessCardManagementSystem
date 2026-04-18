namespace HireMind.Application.Queries.Seurity;
public record SearchPermissionsQuery(SearchFiltersRqDto Filters) : IRequest<SearchFiltersRsDto<PermissionResponseDto>>;

internal class SearchPermissionsHandler : IRequestHandler<SearchPermissionsQuery, SearchFiltersRsDto<PermissionResponseDto>>
{
    private readonly IPermissionRepository _repo;

    public SearchPermissionsHandler(IPermissionRepository repo)
    {
        _repo = repo;
    }

    public async Task<SearchFiltersRsDto<PermissionResponseDto>> Handle(SearchPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _repo.SearchAsync(request.Filters, cancellationToken);
    }
}
