namespace HireMind.Domain.Dtos.ManageJobs;
public class JobQuestionDto
{
    public string QuestionText { get; set; } = null!;
    public int QuestionTypeId { get; set; }
    public bool IsRequired { get; set; } = true;
    public List<AnswerOptionDto> AvailableAnswers { get; set; } = new();
    public int Score { get; set; } = 0;
}
