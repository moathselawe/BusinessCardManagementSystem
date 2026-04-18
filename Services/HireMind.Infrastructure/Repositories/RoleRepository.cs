using HireMind.Domain.Dtos.Security;

namespace HireMind.Infrastructure.Repositories;

public class RoleRepository : BaseRepository<RoleModel>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<RoleModel?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<RoleModel>().Where(x => x.Name == name).FirstOrDefaultAsync();
    }

    public async Task<RoleModel?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<RoleModel>()
            .Include(x => x.RolePermissions)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<List<RoleModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await GetQuery().ToListAsync(cancellationToken);
    }

    public async Task<Guid> Create(RoleModel role, CancellationToken cancellationToken)
    {
        await _dbContext.Set<RoleModel>().AddAsync(role, cancellationToken);
        return role.Id;
    }

    public async Task<SearchFiltersRsDto<GetRoleResponseDto>> SearchAsync(
    SearchFiltersRqDto filters,
    CancellationToken cancellationToken)
    {
        filters ??= new SearchFiltersRqDto(null);

        int pageNumber = filters.PageNumber <= 0 ? 1 : filters.PageNumber;
        int pageSize = filters.PageSize <= 0 ? 5 : filters.PageSize;
        string sortBy = string.IsNullOrWhiteSpace(filters.SortBy) ? "CreatedDate" : filters.SortBy;
        string orderBy = string.IsNullOrWhiteSpace(filters.OrderBy) ? "desc" : filters.OrderBy;

        IQueryable<Role> query = GetQuery()
            .Include(x => x.RolePermissions)
            .ThenInclude(rp => rp.Permission);

        if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
        {
            string term = filters.SearchTerm.Trim();

            query = query.Where(role =>
                role.Name.Contains(term) ||
                role.Description!.Contains(term)
            );
        }

        if (filters.DateSearch.HasValue)
        {
            var date = filters.DateSearch.Value.Date;

            query = query.Where(role =>
                role.CreatedDate == date
            );
        }

        query = (sortBy, orderBy.ToLower()) switch
        {
            ("CreatedDate", "desc") => query.OrderByDescending(x => x.CreatedDate),
            ("CreatedDate", "asc") => query.OrderBy(x => x.CreatedDate),

            ("Name", "desc") => query.OrderByDescending(x => x.Name),
            ("Name", "asc") => query.OrderBy(x => x.Name),

            _ => query.OrderByDescending(x => x.CreatedDate)
        };

        int totalRecords = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(role => new GetRoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedDate = role.CreatedDate,

                PermissionIds = role.RolePermissions
                    .Select(p => p.Permission.Name)
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return new SearchFiltersRsDto<GetRoleResponseDto>(data, totalRecords);
    }

    public async Task<RoleModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<RoleModel>()
            .Include(x => x.RolePermissions)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdQuery(id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null) return false;

        Delete(entity);
        return true;
    }

}
