using HireMind.Domain.Entities.HireMind;

namespace HireMind.Domain.Dtos.ManageJobs;
public class GetJobResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int LocationId { get; set; }
    public string? LocationName { get; set; }

    public int WorkPlaceId { get; set; }
    public string? WorkPlaceName { get; set; }

    public int ContractTypeId { get; set; }
    public string? ContractTypeName { get; set; }

    public int OrganizationTypeId { get; set; }
    public string? OrganizationTypenName { get; set; }

    public int IndustrySectorId { get; set; }
    public string? IndustrySectorName { get; set; }

    public int JobTypeId { get; set; }
    public string? JobTypeName { get; set; }

    public int CompanyId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    // Deserialize QuestionsJson from entity automatically
    public List<JobQuestionDto> Questions { get; set; } = new List<JobQuestionDto>();

    public List<HiringStageDto> HiringStages { get; set; } = new();

    public static GetJobResponseDto FromEntity(Job entity)
    {
        return new GetJobResponseDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,

            LocationId = entity.LocationId,
            LocationName = entity.Location?.CategoryName,

            WorkPlaceId = entity.WorkPlaceId,
            WorkPlaceName = entity.WorkPlace?.CategoryName,

            ContractTypeId = entity.ContractTypeId,
            ContractTypeName = entity.ContractType?.CategoryName,

            OrganizationTypeId = entity.OrganizationTypeId,
            OrganizationTypenName = entity.OrganizationType?.CategoryName,

            IndustrySectorId = entity.IndustrySectorId,
            IndustrySectorName = entity.IndustrySector?.CategoryName,

            JobTypeId = entity.JobTypeId,
            JobTypeName = entity.JobType?.CategoryName,

            CompanyId = entity.CompanyId,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            IsActive = entity.IsActive,

            Questions = JsonSerializer.Deserialize<List<JobQuestionDto>>(entity.QuestionsJson) ?? new List<JobQuestionDto>(),

            HiringStages = entity.HiringStages
                .OrderBy(x => x.StageOrder)
                .Select(x => new HiringStageDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    StageOrder = x.StageOrder,
                    ViaId = x.ViaId,
                    IsActive = x.IsActive,
                    EmailTemplate = x.EmailTemplate ?? string.Empty,

                    InterviewQuestions = x.InterviewQuestions?.Select(q => new JobQuestionDto
                    {
                        QuestionText = q.QuestionText,
                        QuestionTypeId = q.QuestionTypeId,
                        IsRequired = q.IsRequired,
                        Score = q.Score,
                        AvailableAnswers = q.AvailableAnswers?.Select(a => new AnswerOptionDto
                        {
                            Id = a.Id,
                            Text = a.Text,
                            IsPreferredAnswer = a.IsPreferredAnswer
                        }).ToList() ?? new List<AnswerOptionDto>()
                    }).ToList() ?? new List<JobQuestionDto>(),

                    ExamQuestions = x.ExamQuestions?.Select(q => new JobQuestionDto
                    {
                        QuestionText = q.QuestionText,
                        QuestionTypeId = q.QuestionTypeId,
                        IsRequired = q.IsRequired,
                        Score = q.Score,
                        AvailableAnswers = q.AvailableAnswers?.Select(a => new AnswerOptionDto
                        {
                            Id = a.Id,
                            Text = a.Text,
                            IsPreferredAnswer = a.IsPreferredAnswer
                        }).ToList() ?? new List<AnswerOptionDto>()
                    }).ToList() ?? new List<JobQuestionDto>()
                })
                .ToList()
        };
    }
}

