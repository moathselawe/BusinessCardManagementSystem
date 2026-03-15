namespace HireMind.Domain.IRepositories;

public interface IApplicationStageRepository : IRepository<ApplicationStage>
{
    Task<int> AddAsync(ApplicationStage applicationStage, CancellationToken cancellationToken);
}
