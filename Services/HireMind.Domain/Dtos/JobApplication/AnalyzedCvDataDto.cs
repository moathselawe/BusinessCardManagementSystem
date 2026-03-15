namespace HireMind.Domain.Dtos.JobApplication;

public class AnalyzedCvDataDto
{
    public string CvText { get; set; } = "";
    public Dictionary<string, object> Fields { get; set; } = new();
    public double AiScore { get; set; }
}

