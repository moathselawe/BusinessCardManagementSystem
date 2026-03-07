namespace BCMS.Application.Interfaces;

public interface IAnalyzeCvService
{
    Task<AnalyzedCvDataDto> GetAnalyzedCvAsync(IFormFile file, List<JobFieldDto> jobFields, CancellationToken cancellationToken = default);
}
