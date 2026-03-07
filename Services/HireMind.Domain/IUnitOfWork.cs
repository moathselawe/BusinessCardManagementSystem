namespace HireMind.Domain;
public interface IUnitOfWork
{
    IRepository GetRepository<TRepository>() where TRepository : notnull;
    int SaveChanges();
    void SaveWork();
    Task SaveWorkAsync(CancellationToken cancellationToken = default(CancellationToken));
}