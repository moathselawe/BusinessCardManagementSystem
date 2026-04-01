using HireMind.Domain.Entities.Shared;

namespace HireMind.Domain.Entities.HireMind;
public class JobApplication : BaseAuditableEntity
{
    public int JobId { get; private set; }
    public Job Job { get; private set; } = null!;
    public int AnalyzeCvId { get; private set; } 
    public AnalyzeCv AnalyzeCv { get; private set; } = null!;
    public double SystemScore { get; private set; }
    public double TotalScore { get; private set; }
    public string UserAnswersJson { get; private set; } = "[]";
    public PersonalInfo PersonalInfo { get; private set; } = new PersonalInfo();
    public int? CurrentStageId { get; set; }
    public ApplicationStage? CurrentStage { get; private set; }
    public ICollection<ApplicationStage> ApplicationStages { get; set; } = new List<ApplicationStage>();

    public static JobApplication Create(
     int jobId,
     int analyzeCvId,
     double systemScore,
     double totalScore,
     string userAnswersJson,
     PersonalInfo personalInfo)
    {
        return new JobApplication()
        {
            JobId = jobId,
            AnalyzeCvId = analyzeCvId,
            SystemScore = systemScore,
            TotalScore = totalScore,
            UserAnswersJson = userAnswersJson,
            PersonalInfo = personalInfo,
            CreatedDate = DateTime.UtcNow
        };
    }

    public static JobApplication Update(
     int id,
     int jobId,
     int analyzeCvId,
     double systemScore,
     double totalScore,
     string userAnswersJson,
     int currentStageId)
    {
        return new JobApplication()
        {
            Id = id,
            JobId = jobId,
            AnalyzeCvId = analyzeCvId,
            SystemScore = systemScore,
            TotalScore = totalScore,
            UserAnswersJson = userAnswersJson,
            CurrentStageId = currentStageId,
            LastModifiedDate = DateTime.UtcNow
        };
    }

    public void SetCurrentStage(ApplicationStage stage)
    {
        CurrentStage = stage;
        CurrentStageId = stage.Id;
    }

    //public void SetFinalStage(int stageId)
    //{
    //    FinalStageId = stageId;
    //}
}



public class PersonalInfo
{
    public string FullName { get; private set; } = null!;
    public string MobileNumber { get; private set; } = null!;
    public int CountryCodeId { get; private set; }
    public Lookup CountryCode { get; private set; } = null!;
    public string EmailAddress { get; private set; } = null!;

    public static PersonalInfo Create(string fullName, string mobileNumber, int countryCodeId, string emailAddress)
    {
        return new PersonalInfo()
        {
            FullName = fullName,
            MobileNumber = mobileNumber,
            CountryCodeId = countryCodeId,
            EmailAddress = emailAddress
        };
    }
}