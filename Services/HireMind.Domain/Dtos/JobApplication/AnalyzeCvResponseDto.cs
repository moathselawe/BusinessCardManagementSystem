namespace HireMind.Domain.Dtos.JobApplication;

public class AnalyzeCvResponseDto
{
    public AnalyzedCvDataDto AnalyzedCvData { get; set; } = new AnalyzedCvDataDto();
}

public class AnalyzedCvDataDto
{
    public Dictionary<string, object> Fields { get; set; } = new();
}