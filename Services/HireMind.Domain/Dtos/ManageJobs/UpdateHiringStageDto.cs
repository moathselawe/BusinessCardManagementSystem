namespace HireMind.Domain.Dtos.ManageJobs;
public class UpdateHiringStageDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int StageOrder { get; set; }
    public bool IsActive { get; set; } = false;
    public bool IsFinalStage { get; set; } = false;

    // New properties for questions
    public List<JobQuestionDto>? InterviewQuestions { get; set; }
    public List<JobQuestionDto>? ExamQuestions { get; set; }

    // Optional properties if needed
    public int? ViaId { get; set; }
    public string? EmailTemplate { get; set; }
}
