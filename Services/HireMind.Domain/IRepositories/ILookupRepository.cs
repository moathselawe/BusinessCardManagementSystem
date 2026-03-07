namespace HireMind.Domain.IRepositories;

public interface ILookupRepository : IRepository<Lookup>
{
    Task<List<GetLookupByNameDto>> GetAllByNameAsync(string categoryName, CancellationToken cancellationToken);
    Task<Guid> AddAsync(Lookup lookup, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Lookup lookup, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
