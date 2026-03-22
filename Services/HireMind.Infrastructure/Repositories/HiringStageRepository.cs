
using HireMind.Domain.Entities.HireMind;

namespace HireMind.Infrastructure.Repositories;
public class HiringStageRepository : BaseRepository<HiringStage>, IHiringStageRepository
{
    public HiringStageRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task AddRangeAsync(List<HiringStage> stages, CancellationToken cancellationToken)
    {
        _dbContext.Set<HiringStage>().AddRange(stages);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<HiringStage>> GetByJobIdAsync(int jobId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<HiringStage>()
            .Where(x => x.JobId == jobId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(HiringStage stage, CancellationToken cancellationToken)
    {
        await _dbContext.Set<HiringStage>().AddAsync(stage, cancellationToken);
    }

    public async Task UpdateAsync(HiringStage stage, bool trackChanges, CancellationToken cancellationToken)
    {
        _dbContext.Set<HiringStage>().Update(stage);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(List<HiringStage> stages, CancellationToken cancellationToken)
    {
        _dbContext.Set<HiringStage>().RemoveRange(stages);
        await Task.CompletedTask;
    }
}
