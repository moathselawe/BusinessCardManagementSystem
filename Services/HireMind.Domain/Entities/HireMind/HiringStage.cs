namespace HireMind.Domain.Entities.HireMind;
public class HiringStage : BaseAuditableEntity
{
    public int JobId { get; private set; }
    public Job Job { get; set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public int StageOrder { get; private set; }
    public bool IsActive { get; private set; } = false;
    public bool IsFinalStage { get; private set; } = false;
    public int? ViaId { get; private set; }
    public string EmailTemplate { get; private set; } = string.Empty;
    public ICollection<ApplicationStage> ApplicationStages { get; set; } = new List<ApplicationStage>();
    public string InterviewQuestionsJson { get; private set; } = "[]";

    public List<JobQuestion> InterviewQuestions
    {
        get => JsonSerializer.Deserialize<List<JobQuestion>>(InterviewQuestionsJson) ?? new List<JobQuestion>();
        set => InterviewQuestionsJson = JsonSerializer.Serialize(value);
    }

    public string ExamQuestionsJson { get; private set; } = "[]";
    public List<JobQuestion> ExamQuestions
    {
        get => JsonSerializer.Deserialize<List<JobQuestion>>(ExamQuestionsJson) ?? new List<JobQuestion>();
        set => ExamQuestionsJson = JsonSerializer.Serialize(value);
    }

    public static HiringStage Create(
        int jobid,
        string name,
        int stageOrder,
        bool isActive,
        bool isFinalStage = false,
        int? viaId = null,
        string? emailTemplate = null)
    {
        return new HiringStage()
        {
            JobId = jobid,
            Name = name,
            StageOrder = stageOrder,
            IsActive = isActive,
            IsFinalStage = isFinalStage,
            ViaId = viaId,
            EmailTemplate = emailTemplate ?? string.Empty,
            CreatedDate = DateTime.Now
        };
    }

    public void UpdateDetails(string name, int stageOrder, bool isActive, bool isFinalStage, int? viaId, string? emailTemplate)
    {
        Name = name;
        StageOrder = stageOrder;
        IsActive = isActive;
        IsFinalStage = isFinalStage;
        ViaId = viaId;
        EmailTemplate = emailTemplate;
    }

    public void MarkAsFinalStage()
    {
        IsFinalStage = true;
    }
}
