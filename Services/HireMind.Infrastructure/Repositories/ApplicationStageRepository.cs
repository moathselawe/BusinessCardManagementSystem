using HireMind.Domain.Dtos.SharedDtos;
using HireMind.Domain.Entities;
using HireMind.Domain.Enum;
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

    public async Task<int> UpdateBulkApplicationStagesStatusBulkAsync(
    List<int> jobApplicationIds,
    StageStatus newStatus,
    CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<ApplicationStage>()
            .Where(x => jobApplicationIds.Contains(x.JobApplicationId))
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Status, newStatus),
                cancellationToken);
    }

    public async Task AddRangeAsync(List<ApplicationStage> stages, CancellationToken cancellationToken)
    {
        await _dbContext.Set<ApplicationStage>().AddRangeAsync(stages, cancellationToken);
    }

}