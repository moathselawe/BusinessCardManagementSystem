namespace HireMind.Domain.Entities;
public class ApplicationStage : BaseAuditableEntity
{
    public int JobApplicationId { get; set; }
    public JobApplication JobApplication { get; set; } = null!;
    public int HiringStageId { get; set; }
    public HiringStage HiringStage { get; set; } = null!;
    public StageStatus Status { get; set; } = StageStatus.New;
    public int? Score { get; set; }
    public string? Notes { get; set; }

    public static ApplicationStage Create(
     int jobApplicationId,
     int hiringStageId,
     StageStatus status)
    {
        return new ApplicationStage()
        {
            JobApplicationId = jobApplicationId,
            HiringStageId = hiringStageId,
            Status = status,
            CreatedDate = DateTime.UtcNow
        };
    }

    public static ApplicationStage Update(int id, int jobApplicationId, int hiringStageId, StageStatus status, int? score, string? notes)
    {
        return new ApplicationStage()
        {
            Id = id,
            JobApplicationId = jobApplicationId,
            HiringStageId = hiringStageId,
            Status = status,
            Score = score,
            Notes = notes,
            LastModifiedDate = DateTime.UtcNow
        };
    }

    public static ApplicationStage UpdateApplicationStageStatus(int id, StageStatus NewStatus)
    {
        return new ApplicationStage()
        { 
            Id = id,
            Status = NewStatus,
        };
    }
}