using BCMS.Domain.Dtos.SharedDtos;

namespace BCMS.Domain.IRepositories;

public interface IJobRepository : IRepository<Job>
{
    Task<Guid> AddAsync(Job job, CancellationToken cancellationToken);
    Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Job job, bool ActivationOnly = false, CancellationToken cancellationToken = default);
    Task<SearchFiltersRsDto<GetJobResponseDto>> SearchAsync(SearchFiltersRqDto filters, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
