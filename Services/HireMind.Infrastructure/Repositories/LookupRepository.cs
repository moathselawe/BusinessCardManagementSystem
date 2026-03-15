namespace HireMind.Infrastructure.Repositories;

public class LookupRepository : BaseRepository<Lookup>, ILookupRepository
{
    public LookupRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<SearchFiltersRsDto<GetLookupDto>> SearchAsync(
    SearchFiltersRqDto filters,
    CancellationToken cancellationToken)
    {
        filters ??= new SearchFiltersRqDto(null);

        int pageNumber = filters.PageNumber <= 0 ? 1 : filters.PageNumber;
        int pageSize = filters.PageSize <= 0 ? 5 : filters.PageSize;
        string sortBy = string.IsNullOrWhiteSpace(filters.SortBy) ? "CreatedDate" : filters.SortBy;
        string orderBy = string.IsNullOrWhiteSpace(filters.OrderBy) ? "desc" : filters.OrderBy;

        // Use IQueryable<Lookup>
        IQueryable<Lookup> query = GetQuery();

        // Filter by search term
        if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
        {
            string term = filters.SearchTerm.Trim();
            query = query.Where(l => l.CategoryName.Contains(term));
        }

        // Filter by date
        if (filters.DateSearch.HasValue)
        {
            var date = filters.DateSearch.Value.Date;
            query = query.Where(l => l.CreatedDate.Date == date);
        }

        // Apply sorting
        query = (sortBy, orderBy.ToLower()) switch
        {
            ("CreatedDate", "desc") => query.OrderByDescending(x => x.CreatedDate),
            ("CreatedDate", "asc") => query.OrderBy(x => x.CreatedDate),
            ("CategoryName", "desc") => query.OrderByDescending(x => x.CategoryName),
            ("CategoryName", "asc") => query.OrderBy(x => x.CategoryName),
            _ => query.OrderByDescending(x => x.CreatedDate)
        };

        // Get total count
        int totalRecords = await query.CountAsync(cancellationToken);

        // Project to DTO with parent name using Include in the final query
        var data = await query
            .Include(l => l.Parent) // Include parent here
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new GetLookupDto
            {
                Id = l.Id,
                CategoryName = l.CategoryName,
                ParentId = l.ParentId,
                ParentName = l.Parent != null ? l.Parent.CategoryName : null
            })
            .ToListAsync(cancellationToken);

        return new SearchFiltersRsDto<GetLookupDto>(data, totalRecords);
    }

    public async Task<Lookup?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await GetByIdQuery(id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<GetLookupDto>> GetAllLookupParentsAsync(CancellationToken cancellationToken)
    {
        var parents = await GetQuery()
            .Where(l => l.ParentId == null)
            .Select(l => new GetLookupDto
            {
                Id = l.Id,
                CategoryName = l.CategoryName,
                ParentId = null
            })
            .ToListAsync(cancellationToken);

        return parents;
    }

    public async Task<List<GetAllLookupsPartenersAndChildrensDto>> GetAllParentsAndChildsLookupsAsync(CancellationToken cancellationToken)
    {
        // Get all lookups from the repository query
        var allLookups = await GetQuery()
            .Select(l => new GetLookupDto
            {
                Id = l.Id,
                CategoryName = l.CategoryName,
                ParentId = l.ParentId
            })
            .ToListAsync(cancellationToken);

        // Filter parents and assign children
        var parents = allLookups
            .Where(p => p.ParentId == null)
            .Select(p => new GetAllLookupsPartenersAndChildrensDto
            {
                Id = p.Id,
                CategoryName = p.CategoryName,
                ParentId = null,
                ParentName = null,
                Children = allLookups
                    .Where(c => c.ParentId == p.Id)
                    .ToList()
            })
            .ToList();

        return parents;
    }

    public async Task<List<GetLookupDto>> GetAllByNameAsync(string categoryName, CancellationToken cancellationToken)
    {
        var parent = await GetQuery()
            .FirstOrDefaultAsync(l => l.CategoryName == categoryName && l.ParentId == null, cancellationToken);

        if (parent == null)
            return new List<GetLookupDto>();

        var children = await GetQuery()
            .Where(l => l.ParentId == parent.Id)
            .Select(c => new GetLookupDto
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                ParentId = c.ParentId
            })
            .ToListAsync(cancellationToken);

        return children;
    }

    //public async Task<List<GetLookupDto>> GetAllByNameAsync(string categoryName, CancellationToken cancellationToken)
    //{
    //    // 1️⃣ Find the parent category
    //    var parent = await GetQuery()
    //        .Where(l => l.CategoryName == categoryName && l.ParentId == null)
    //        .FirstOrDefaultAsync(cancellationToken);

    //    if (parent == null)
    //        return new List<GetLookupDto>();

    //    // 2️⃣ Get all child entries
    //    var children = await GetQuery()
    //        .Where(l => l.ParentId == parent.Id)
    //        .ToListAsync(cancellationToken);

    //    // 3️⃣ Map to DTO
    //    var dtos = children
    //        .Select(c => new GetLookupDto
    //        {
    //            Id = c.Id,
    //            CategoryName = c.CategoryName
    //        })
    //        .ToList();

    //    return dtos;
    //}

    //public async Task<int> AddAsync(Lookup lookup, CancellationToken cancellationToken)
    //{
    //    Add(lookup);
    //    return await Task.FromResult(lookup.Id);
    //}

    public async Task<int> AddAsync(Lookup lookup, CancellationToken cancellationToken)
    {
        Add(lookup); // add to DbContext
        await _dbContext.SaveChangesAsync(cancellationToken); // save to DB
        return lookup.Id; // now Id is generated by DB
    }

    public async Task<bool> UpdateAsync(Lookup lookup, CancellationToken cancellationToken = default)
    {
        var existing = await GetQuery().FirstOrDefaultAsync(l => l.Id == lookup.Id, cancellationToken);
        
        if (existing == null)
            return false;

        Update(lookup);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdQuery(id).FirstOrDefaultAsync(cancellationToken);
        
        if (entity == null) 
            return false;

        if (entity.ParentId == null)
        {
            var hasChildren = await GetQuery().AnyAsync(l => l.ParentId == entity.Id, cancellationToken);
            if (hasChildren)
                return false; 
        }

        Delete(entity);

        return true;
    }

    public async Task<Lookup?> GetByIdWithParentAsync(int id, CancellationToken cancellationToken)
    {
        return await GetQuery()
            .Include(x => x.Parent)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}