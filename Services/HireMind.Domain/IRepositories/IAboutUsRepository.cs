namespace HireMind.Domain.IRepositories;

public interface IAboutUsRepository : IRepository<AboutUs>
{
    Task<List<AboutUs>> GetAllAsync(bool isActive,CancellationToken cancellationToken);
}
