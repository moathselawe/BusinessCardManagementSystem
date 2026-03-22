using HireMind.Domain.Entities.HireMind;

namespace HireMind.Domain.IRepositories;
public interface IHiringStageRepository : IRepository<HiringStage>
{
    Task AddRangeAsync(List<HiringStage> stages, CancellationToken cancellationToken);

    Task<List<HiringStage>> GetByJobIdAsync(int jobId, CancellationToken cancellationToken);

    Task AddAsync(HiringStage stage, CancellationToken cancellationToken);

    Task UpdateAsync(HiringStage stage, bool trackChanges, CancellationToken cancellationToken);

    Task DeleteRangeAsync(List<HiringStage> stages, CancellationToken cancellationToken);
}