using HireMind.Domain.Dtos.SharedDtos;
using Microsoft.EntityFrameworkCore;

namespace HireMind.Infrastructure.Repositories;

public class ApplicationStageRepository : BaseRepository<ApplicationStage>, IApplicationStageRepository
{
    public ApplicationStageRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<int> AddAsync(ApplicationStage applicationStage, CancellationToken cancellationToken)
    {
        Add(applicationStage);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(applicationStage.Id);
    }
}
