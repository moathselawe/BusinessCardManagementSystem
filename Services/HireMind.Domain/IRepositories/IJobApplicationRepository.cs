using HireMind.Domain.Dtos.JobApplication;

namespace HireMind.Domain.IRepositories;

public interface IJobApplicationRepository : IRepository<JobApplication>
{
    Task<int> AddAsync(JobApplication application, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(JobApplication application, CancellationToken cancellationToken = default);
    Task<bool> CheckApplicationByEmailAndJobId(string email, int jobId, CancellationToken cancellationToken);
    Task<List<JobApplicationDto>> GetAllJobApplicationsByJobIdAsync(int jobid, CancellationToken cancellationToken);
    Task<JobApplication> GetByIdAsync(int jobApplicationId, CancellationToken cancellationToken);
    Task<JobApplication> GetJobApplicationByIdWithAnalyzedCVAsync(int jobApplicationId, CancellationToken cancellationToken);
    Task<List<JobApplication>> GetByIdsWithCurrentStageAsync(List<int> ids, CancellationToken cancellationToken);
}
