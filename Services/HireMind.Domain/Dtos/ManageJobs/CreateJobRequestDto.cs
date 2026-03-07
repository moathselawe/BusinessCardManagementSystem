namespace HireMind.Domain.Dtos.ManageJobs;
public class CreateJobRequestDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int LocationId { get; set; }
    public int WorkPlaceId { get; set; }
    public int ContractTypeId { get; set; }
    public int OrganizationTypeId { get; set; }
    public int IndustrySectorId { get; set; }
    public int JobTypeId { get; set; }
    public int CompanyId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public List<JobQuestionDto> Questions { get; set; } = new List<JobQuestionDto>();
}