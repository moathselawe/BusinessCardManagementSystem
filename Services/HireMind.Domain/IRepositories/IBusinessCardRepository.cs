using HireMind.Domain.Dtos.SharedDtos;

namespace HireMind.Domain.IRepositories;

public interface IBusinessCardRepository : IRepository<BusinessCard>
{
    Task<SearchFiltersRsDto<BusinessCardDto>> SearchAsync(SearchFiltersRqDto filters, CancellationToken cancellationToken);
    Task<List<BusinessCard>> GetAllAsync(CancellationToken cancellationToken);
    Task<BusinessCard?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Guid> AddAsync(BusinessCard businessCard, CancellationToken cancellationToken);
    public Task<int> AddManyAsync(List<BusinessCard> businessCards, CancellationToken cancellationToken);
    Task<BusinessCard?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(BusinessCard businessCard, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<List<BusinessCard>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
}
