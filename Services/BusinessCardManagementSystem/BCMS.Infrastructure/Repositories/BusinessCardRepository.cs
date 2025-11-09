using BCMS.Domain.Dtos;

namespace BCMS.Infrastructure.Repositories;

public class BusinessCardRepository : BaseRepository<BusinessCard>, IBusinessCardRepository
{
    public BusinessCardRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<List<BusinessCard>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await GetQuery().ToListAsync(cancellationToken);
    }

    public Task<Guid> AddAsync(BusinessCard businessCard, CancellationToken cancellationToken)
    {
        Add(businessCard); 
        return Task.FromResult(businessCard.Id); 
    }

    public Task<int> AddManyAsync(List<BusinessCard> businessCards, CancellationToken cancellationToken)
    {
        AddMany(businessCards);
        return Task.FromResult(businessCards.Count);
    }

    public async Task<BusinessCard?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await GetByIdQuery(id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(BusinessCard businessCard, CancellationToken cancellationToken)
    {
        var existing = await GetByIdQuery(businessCard.Id).FirstOrDefaultAsync(cancellationToken);

        if (existing == null)
            return false;

        Update(businessCard); 
        return true; 
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdQuery(id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null) return false;

        Delete(entity); 
        return true;
    }

    public async Task<List<BusinessCard>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        return await GetQuery()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }
    public async Task<SearchFiltersRsDto<BusinessCardDto>> SearchAsync(SearchFiltersRqDto filters, CancellationToken cancellationToken)
    {
        filters ??= new SearchFiltersRqDto(null);

        int pageNumber = filters.PageNumber <= 0 ? 1 : filters.PageNumber;
        int pageSize = filters.PageSize <= 0 ? 5 : filters.PageSize;
        string sortBy = string.IsNullOrWhiteSpace(filters.SortBy) ? "CreatedDate" : filters.SortBy;
        string orderBy = string.IsNullOrWhiteSpace(filters.OrderBy) ? "desc" : filters.OrderBy;

        var query = GetQuery();

        if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
        {
            string term = filters.SearchTerm.Trim();
            query = query.Where(card =>
                card.ArabicName.Contains(term) ||
                card.EnglishName.Contains(term) ||
                card.Phone.Contains(term) ||
                card.Email.Contains(term) ||
                card.Address.Contains(term)
            );
        }

        if (filters.DateSearch.HasValue)
        {
            var date = filters.DateSearch.Value.Date;
            query = query.Where(card => card.CreatedDate.Date == date);
        }

        query = (sortBy, orderBy.ToLower()) switch
        {
            ("CreatedDate", "desc") => query.OrderByDescending(x => x.CreatedDate),
            ("CreatedDate", "asc") => query.OrderBy(x => x.CreatedDate),
            ("ArabicName", "desc") => query.OrderByDescending(x => x.ArabicName),
            ("ArabicName", "asc") => query.OrderBy(x => x.ArabicName),
            ("EnglishName", "desc") => query.OrderByDescending(x => x.EnglishName),
            ("EnglishName", "asc") => query.OrderBy(x => x.EnglishName),
            _ => query.OrderByDescending(x => x.CreatedDate)
        };

        int totalRecords = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(card => new BusinessCardDto(
                card.Id,
                card.ArabicName,
                card.EnglishName,
                card.DateOfBirth,
                card.Email,
                card.Phone,
                card.Logo,
                card.Address
            ))
            .ToListAsync(cancellationToken);

        return new SearchFiltersRsDto<BusinessCardDto>(data, totalRecords);
    }

}
