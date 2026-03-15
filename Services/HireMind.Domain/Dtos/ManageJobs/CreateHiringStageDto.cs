namespace HireMind.Domain.Dtos.ManageJobs;
public class CreateHiringStageDto
{
    public string Name { get; set; } = string.Empty;

    public int StageOrder { get; set; }

    public int? ViaId { get; set; }
    public string? EmailTemplate { get; set; }
    public List<JobQuestionDto>? InterviewQuestions { get; set; }
    public List<JobQuestionDto>? ExamQuestions { get; set; }
}
