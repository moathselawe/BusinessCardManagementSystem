namespace HireMind.Application.Interfaces;

public interface IAnalyzeCvService
{
    // Task<AnalyzedCvDataDto> GetAnalyzedCvAsync(IFormFile file, List<JobFieldDto> jobFields, CancellationToken cancellationToken = default);
    Task<AnalyzedCvDataDto> GetAnalyzedCvAsync(
         IFormFile file,
         Job job,
         CancellationToken cancellationToken = default);
}
