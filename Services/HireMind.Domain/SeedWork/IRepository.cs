namespace HireMind.Domain.SeedWork;
public interface IRepository : IDisposable, IAsyncDisposable
{
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IRepository<TEntity> : IRepository where TEntity : class
{
    IQueryable<TEntity> GetByIdQuery(Guid id, bool excludeDeleted = true, bool asNoTracking = true);
    IQueryable<TEntity> GetQuery(bool excludeDeleted = true, bool asNoTracking = true);
    void Add(TEntity entity);
    void AddMany(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateMany(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    void DeleteMany(IEnumerable<TEntity> entities);
}
