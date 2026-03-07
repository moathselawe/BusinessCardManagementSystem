namespace HireMind.Infrastructure.Repositories;

public class LookupRepository : BaseRepository<Lookup>, ILookupRepository
{
    public LookupRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<List<GetLookupByNameDto>> GetAllByNameAsync(string categoryName, CancellationToken cancellationToken)
    {
        // 1️⃣ Find the parent category
        var parent = await GetQuery()
            .Where(l => l.CategoryName == categoryName && l.ParentId == null)
            .FirstOrDefaultAsync(cancellationToken);

        if (parent == null)
            return new List<GetLookupByNameDto>();

        // 2️⃣ Get all child entries
        var children = await GetQuery()
            .Where(l => l.ParentId == parent.Id)
            .ToListAsync(cancellationToken);

        // 3️⃣ Map to DTO
        var dtos = children
            .Select(c => new GetLookupByNameDto
            {
                Id = c.Id,
                CategoryName = c.CategoryName
            })
            .ToList();

        return dtos;
    }

    public async Task<Guid> AddAsync(Lookup lookup, CancellationToken cancellationToken)
    {
        Add(lookup);
        return await Task.FromResult(lookup.Id);
    }

    public async Task<bool> UpdateAsync(Lookup lookup, CancellationToken cancellationToken = default)
    {
        var existing = await GetQuery().FirstOrDefaultAsync(l => l.Id == lookup.Id, cancellationToken);
        
        if (existing == null)
            return false;

        Update(lookup);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
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
}