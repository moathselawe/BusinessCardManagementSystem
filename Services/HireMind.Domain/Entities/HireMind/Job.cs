using HireMind.Domain.Entities.Shared;

namespace HireMind.Domain.Entities.HireMind;
public class Job : BaseAuditableEntity
{
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public int LocationId { get; private set; }
    public Lookup Location { get; private set; } = null!;
    public int JobTypeId { get; private set; }
    public Lookup JobType { get; private set; } = null!;
    public int WorkPlaceId { get; private set; }
    public Lookup WorkPlace { get; private set; } = null!;
    public int ContractTypeId { get; private set; }
    public Lookup ContractType { get; private set; } = null!;
    public int OrganizationTypeId { get; private set; }
    public Lookup OrganizationType { get; private set; } = null!;
    public int IndustrySectorId { get; private set; }
    public Lookup IndustrySector { get; private set; } = null!;
    public int CompanyId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string QuestionsJson { get; private set; } = "[]";
    public List<JobQuestion> Questions
    {
        get => JsonSerializer.Deserialize<List<JobQuestion>>(QuestionsJson) ?? new List<JobQuestion>();
        set => QuestionsJson = JsonSerializer.Serialize(value);
    }
    public ICollection<AnalyzeCv> AnalyzeCvs { get; private set; } = new List<AnalyzeCv>();
    public ICollection<JobApplication> JobApplications { get; private set; } = new List<JobApplication>();
    public ICollection<HiringStage> HiringStages { get; private set; } = new List<HiringStage>();

    public static Job Create(
    string title,
    string description,
    int locationId,
    int workPlaceId,
    int contractTypeId,
    int organizationTypeId,
    int industrySectorId,
    int jobTypeId,
    int companyId,
    DateTime startDate,
    DateTime? endDate,
    bool isActive,
    List<JobQuestion>? questions = null)
    {
        return new Job()
        {
            Title = title,
            Description = description,
            LocationId = locationId,
            WorkPlaceId = workPlaceId,
            ContractTypeId = contractTypeId,
            OrganizationTypeId = organizationTypeId,
            IndustrySectorId = industrySectorId,
            JobTypeId = jobTypeId,
            CompanyId = companyId,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = isActive,
            Questions = questions ?? new List<JobQuestion>(),
            CreatedDate = DateTime.UtcNow
        };
    }

    public static Job Update(
        int id,
        string title,
        string description,
        int locationId,
        int workPlaceId,
        int contractTypeId,
        int organizationTypeId,
        int industrySectorId,
        int jobTypeId,
        int companyId,
        DateTime startDate,
        DateTime? endDate,
        bool isActive,
        List<JobQuestion>? questions = null)
    {
        return new Job()
        {
            Id = id,
            Title = title,
            Description = description,
            LocationId = locationId,
            WorkPlaceId = workPlaceId,
            ContractTypeId = contractTypeId,
            OrganizationTypeId = organizationTypeId,
            IndustrySectorId = industrySectorId,
            JobTypeId = jobTypeId,
            CompanyId = companyId,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = isActive,
            Questions = questions ?? new List<JobQuestion>(),
            LastModifiedDate = DateTime.UtcNow
        };
    }

    public static Job UpdateActivation(int id, bool isActive)
    {
        return new Job()
        {
            Id = id,
            IsActive = isActive
        };
    }
}

public class JobQuestion
{
    public string QuestionText { get; set; } = null!;
    public int QuestionTypeId { get; set; }
    public bool IsRequired { get; set; } = true;
    public List<AnswerOption> AvailableAnswers { get; set; } = new();
    public int Score { get; set; } = 0;
}

public class AnswerOption
{
    public string Id { get; set; } = null!;
    public string Text { get; set; } = null!;
    public bool IsPreferredAnswer { get; set; }
}




