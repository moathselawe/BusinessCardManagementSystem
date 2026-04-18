
namespace HireMind.Domain.IRepositories;

public interface IRoleRepository: IRepository<Role>
{
    Task<Role?> GetByName(string name, CancellationToken cancellationToken);
    Task<Role?> GetById(Guid id, CancellationToken cancellationToken);
    Task<List<Role>> GetAllAsync(CancellationToken cancellationToken);
    Task <Guid>Create(Role role, CancellationToken cancellationToken);
    Task<SearchFiltersRsDto<GetRoleResponseDto>> SearchAsync(SearchFiltersRqDto filters, CancellationToken cancellationToken);
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
