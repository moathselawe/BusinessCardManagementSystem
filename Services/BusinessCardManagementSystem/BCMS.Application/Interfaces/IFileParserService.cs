namespace BCMS.Application.Interfaces;

public interface IFileParserService
{
    Task<List<BusinessCardPreviewDto>> ParseFileAsync(IFormFile file);
}