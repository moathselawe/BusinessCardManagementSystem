using HireMind.Domain.Dtos.ApplicationStage;
using HireMind.Domain.Dtos.JobApplication;
using HireMind.Domain.Entities.HireMind;

namespace HireMind.Domain.IRepositories;

public interface IApplicationStageRepository : IRepository<ApplicationStage>
{
    Task<int> AddAsync(ApplicationStage applicationStage, CancellationToken cancellationToken);
    Task<int> UpdateBulkApplicationStagesStatusBulkAsync(List<int> jobApplicationIds, StageStatus newStatus, CancellationToken cancellationToken = default);
    Task AddRangeAsync(List<ApplicationStage> stages, CancellationToken cancellationToken);
    Task<List<JobApplicationDto>> SearchJobApplicationsAsync(
        SearchJobApplicationsRequestDto request,
        CancellationToken cancellationToken);
}
