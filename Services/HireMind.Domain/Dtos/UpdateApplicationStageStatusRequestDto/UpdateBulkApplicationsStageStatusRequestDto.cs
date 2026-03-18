namespace HireMind.Domain.Dtos.UpdateApplicationStageStatusRequestDto;

public class UpdateBulkApplicationsStageStatusRequestDto
{
    public List<int> Ids { get; set; } = new List<int>(); 

    public int JobId { get; set; }
    public StageStatus NewStatus { get; set; }
}
