namespace HireMind.Infrastructure.Repositories;

public class AboutUsRepository : BaseRepository<AboutUs>, IAboutUsRepository
{
    public AboutUsRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<List<AboutUs>> GetAllAsync(bool isActive, CancellationToken cancellationToken)
    {
        return await GetQuery()
            .Where(x => x.IsActive == isActive)
            .OrderBy(x => x.Order)   
            .ToListAsync(cancellationToken);
    }
}