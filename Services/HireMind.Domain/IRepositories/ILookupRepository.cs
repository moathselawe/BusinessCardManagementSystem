using HireMind.Domain.Entities.Shared;

namespace HireMind.Domain.IRepositories;

public interface ILookupRepository : IRepository<Lookup>
{
    Task<SearchFiltersRsDto<GetLookupDto>> SearchAsync(SearchFiltersRqDto filters, CancellationToken cancellationToken);
    Task<Lookup?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<GetLookupDto>> GetAllByNameAsync(string categoryName, CancellationToken cancellationToken);
    Task<List<GetLookupDto>> GetAllLookupParentsAsync(CancellationToken cancellationToken);
    Task<int> AddAsync(Lookup lookup, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Lookup lookup, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<Lookup?> GetByIdWithParentAsync(int id, CancellationToken cancellationToken);
    Task<List<GetAllLookupsPartenersAndChildrensDto>> GetAllParentsAndChildsLookupsAsync(CancellationToken cancellationToken);
    
    }
