using System;
using System.Diagnostics;

namespace HireMind.Infrastructure.SeedWork;
public abstract class BaseRepository : IRepository
{
    private bool _disposed;
    protected readonly DbContext _dbContext;

    protected BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int SaveChanges() => _dbContext.SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                _dbContext.Dispose();

            _disposed = true;
        }
    }

    ~BaseRepository()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}


public abstract class BaseRepository<TEntity> : BaseRepository where TEntity : class
{
    private readonly bool _isAuditableEntityType;

    protected BaseRepository(DbContext dbContext)
        : base(dbContext)
    {
        _isAuditableEntityType = typeof(TEntity).IsAssignableTo(typeof(BaseAuditableEntity));
    }

    public virtual IQueryable<TEntity> GetQuery(bool excludeDeleted = true, bool asNoTracking = true)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (_isAuditableEntityType && excludeDeleted)
            query = query.Where(x => !(x as BaseAuditableEntity)!.IsDeleted);

        return query;
    }

    public virtual IQueryable<TEntity> GetByIdQuery(int id, bool excludeDeleted = true, bool asNoTracking = true)
        => GetQuery(excludeDeleted, asNoTracking).Where(x => (x as BaseEntity<int>)!.Id == id);

    public virtual void Add(TEntity entity)
    {
        if (_isAuditableEntityType)
        {
            var auditable = (entity as BaseAuditableEntity)!;
            auditable.CreatedDate = DateTime.UtcNow;
        }
        _dbContext.Set<TEntity>().Add(entity);
    }

    public virtual void AddMany(IEnumerable<TEntity> entities)
    {
        if (_isAuditableEntityType)
        {
            foreach (var entity in entities)
                (entity as BaseAuditableEntity)!.CreatedDate = DateTime.UtcNow;
        }
        _dbContext.Set<TEntity>().AddRange(entities);
    }

    public virtual void Update(TEntity entity)
    {
        if (_isAuditableEntityType)
        {
            (entity as BaseAuditableEntity)!.LastModifiedDate = DateTime.UtcNow;
        }
        _dbContext.Set<TEntity>().Update(entity);
    }

    public virtual void UpdateMany(IEnumerable<TEntity> entities)
    {
        if (_isAuditableEntityType)
        {
            foreach (var entity in entities)
                (entity as BaseAuditableEntity)!.LastModifiedDate = DateTime.UtcNow;
        }
        _dbContext.Set<TEntity>().UpdateRange(entities);
    }

    public virtual void Delete(TEntity entity)
    {
        if (_isAuditableEntityType)
        {
            var auditable = (entity as BaseAuditableEntity)!;
            auditable.IsDeleted = true;
            auditable.DeleteDate = DateTime.UtcNow;

            _dbContext.Set<TEntity>().Update(entity);
        }
        else
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }
    }

    public virtual void DeleteMany(IEnumerable<TEntity> entities)
    {
        if (_isAuditableEntityType)
        {
            foreach (var entity in entities)
            {
                var auditable = (entity as BaseAuditableEntity)!;
                auditable.IsDeleted = true;
                auditable.DeleteDate = DateTime.UtcNow;
            }
            _dbContext.Set<TEntity>().UpdateRange(entities);
        }
        else
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }
    }
}
