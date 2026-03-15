namespace HireMind.Domain.Dtos.JobApplication;

public class AnalyzeCvResponseDto
{
    public int AnalyzeCvId { get; set; }
    public AnalyzedCvDataDto AnalyzedCvData { get; set; } = new AnalyzedCvDataDto();
}

