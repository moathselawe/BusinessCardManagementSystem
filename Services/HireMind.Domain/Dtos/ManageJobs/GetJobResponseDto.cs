namespace HireMind.Domain.Dtos.ManageJobs;
public class GetJobResponseDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int LocationId { get; set; }
    //new
    public int WorkPlaceId { get; set; }
    public int ContractTypeId { get; set; }
    public int OrganizationTypeId { get; set; }
    public int IndustrySectorId { get; set; }
    //new

    public int JobTypeId { get; set; }

    public int CompanyId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    // Deserialize QuestionsJson from entity automatically
    public List<JobQuestionDto> Questions { get; set; } = new List<JobQuestionDto>();

    // Optional: factory method from entity
    public static GetJobResponseDto FromEntity(jobModel entity)
    {
        return new GetJobResponseDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            LocationId = entity.LocationId,
            ContractTypeId = entity.ContractTypeId,
            IndustrySectorId = entity.IndustrySectorId,
            WorkPlaceId = entity.WorkPlaceId,
            OrganizationTypeId = entity.OrganizationTypeId,
            JobTypeId = entity.JobTypeId,
            CompanyId = entity.CompanyId,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            IsActive = entity.IsActive,
            Questions = JsonSerializer.Deserialize<List<JobQuestionDto>>(entity.QuestionsJson) ?? new List<JobQuestionDto>()
        };
    }
}

