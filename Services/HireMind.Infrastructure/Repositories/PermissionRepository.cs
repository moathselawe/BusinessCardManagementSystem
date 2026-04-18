using HireMind.Domain.Dtos.Security;
using HireMind.Domain.SeedWork;

namespace HireMind.Infrastructure.Repositories;

public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
{
    public PermissionRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    public async Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await GetQuery().ToListAsync(cancellationToken);
    }
    public async Task<SearchFiltersRsDto<PermissionResponseDto>> SearchAsync(
    SearchFiltersRqDto filters,
    CancellationToken cancellationToken)
    {
        filters ??= new SearchFiltersRqDto(null);

        int pageNumber = filters.PageNumber <= 0 ? 1 : filters.PageNumber;
        int pageSize = filters.PageSize <= 0 ? 5 : filters.PageSize;
        string sortBy = string.IsNullOrWhiteSpace(filters.SortBy) ? "CreatedDate" : filters.SortBy;
        string orderBy = string.IsNullOrWhiteSpace(filters.OrderBy) ? "desc" : filters.OrderBy;

        IQueryable<Permission> query = GetQuery();

        if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
        {
            string term = filters.SearchTerm.Trim();

            query = query.Where(permission =>
                permission.Name.Contains(term) ||
                permission.Code.Contains(term) ||
                (permission.Description != null && permission.Description.Contains(term))
            );
        }

        if (filters.DateSearch.HasValue)
        {
            var date = filters.DateSearch.Value.Date;

            query = query.Where(permission =>
                permission.CreatedDate == date
            );
        }

        query = (sortBy, orderBy.ToLower()) switch
        {
            ("CreatedDate", "desc") => query.OrderByDescending(x => x.CreatedDate),
            ("CreatedDate", "asc") => query.OrderBy(x => x.CreatedDate),

            ("Name", "desc") => query.OrderByDescending(x => x.Name),
            ("Name", "asc") => query.OrderBy(x => x.Name),

            ("Code", "desc") => query.OrderByDescending(x => x.Code),
            ("Code", "asc") => query.OrderBy(x => x.Code),

            _ => query.OrderByDescending(x => x.CreatedDate)
        };

        int totalRecords = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(permission => new PermissionResponseDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Code = permission.Code,
                Description = permission.Description,
                CreatedDate = permission.CreatedDate
            })
            .ToListAsync(cancellationToken);

        return new SearchFiltersRsDto<PermissionResponseDto>(data, totalRecords);
    }
}
