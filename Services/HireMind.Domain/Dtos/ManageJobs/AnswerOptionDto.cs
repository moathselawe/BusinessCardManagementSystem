namespace HireMind.Domain.Dtos.ManageJobs;
public class AnswerOptionDto
{
    public string Id { get; set; } = null!;
    public string Text { get; set; } = null!;
    public bool IsPreferredAnswer { get; set; }
}