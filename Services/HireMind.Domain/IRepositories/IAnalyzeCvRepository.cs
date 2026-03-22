using HireMind.Domain.Entities.HireMind;

namespace HireMind.Domain.IRepositories;

public interface IAnalyzeCvRepository : IRepository<AnalyzeCv>
{
    Task<int> AddAsync(AnalyzeCv cv, CancellationToken cancellationToken);
    Task<AnalyzeCv?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(AnalyzeCv analyzeCv, bool IsActivationOnly = false, CancellationToken cancellationToken = default);
    Task<AnalyzeCv> GetAnalyzedCvAsync(string emailAddress,int jobId, CancellationToken cancellationToken);
}
