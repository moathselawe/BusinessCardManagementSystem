namespace BCMS.Infrastructure;
public class UnitOfWork : IUnitOfWork
{
    private bool disposedValue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction? _dbContextTransaction = null;

    public UnitOfWork(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
        _dbContextTransaction = dbContext.Database.BeginTransaction();
    }
    public IRepository GetRepository<TRepository>() where TRepository : notnull
    {
        return (IRepository)_serviceProvider.GetRequiredService<TRepository>();
    }
    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }
    public void SaveWork()
    {
        try
        {
            _dbContext.SaveChanges();
            _dbContextTransaction!.Commit();
        }
        catch (Exception ex) {
            _dbContextTransaction!.RollbackAsync();
            throw;
        }
    }
    public async Task SaveWorkAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _dbContextTransaction!.CommitAsync();
        }
        catch (Exception ex)
        {
           await _dbContextTransaction!.RollbackAsync();
            throw;
        }
    }
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _dbContextTransaction!.Rollback();
            }
            disposedValue = true;
        }

    }
}