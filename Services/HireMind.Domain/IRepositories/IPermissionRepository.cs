
namespace HireMind.Domain.IRepositories;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken);
    Task<SearchFiltersRsDto<PermissionResponseDto>> SearchAsync(SearchFiltersRqDto filters, CancellationToken cancellationToken);
}
