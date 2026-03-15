using HireMind.Domain.Dtos.SharedDtos;

namespace HireMind.Domain.IRepositories;

public interface IJobRepository : IRepository<Job>
{
    Task<int> AddAsync(Job job, CancellationToken cancellationToken);
    Task<Job?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Job job, bool ActivationOnly = false, CancellationToken cancellationToken = default);
    Task<SearchFiltersRsDto<GetJobResponseDto>> SearchAsync(SearchFiltersRqDto filters, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
