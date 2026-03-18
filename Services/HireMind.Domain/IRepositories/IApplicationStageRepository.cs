namespace HireMind.Domain.IRepositories;

public interface IApplicationStageRepository : IRepository<ApplicationStage>
{
    Task<int> AddAsync(ApplicationStage applicationStage, CancellationToken cancellationToken);
    Task<int> UpdateBulkApplicationStagesStatusBulkAsync(List<int> jobApplicationIds, StageStatus newStatus, CancellationToken cancellationToken = default);
    Task AddRangeAsync(List<ApplicationStage> stages, CancellationToken cancellationToken);
}
