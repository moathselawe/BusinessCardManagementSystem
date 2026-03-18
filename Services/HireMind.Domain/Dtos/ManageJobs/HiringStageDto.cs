namespace HireMind.Domain.Dtos.ManageJobs;
public class HiringStageDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int StageOrder { get; set; }

    public bool IsActive { get; set; }
    public bool IsFinalStage { get; set; } = false;

    public int? ViaId { get; set; }

    public string EmailTemplate { get; set; } = string.Empty;

    public List<JobQuestionDto> InterviewQuestions { get; set; } = new();

    public List<JobQuestionDto> ExamQuestions { get; set; } = new();
}