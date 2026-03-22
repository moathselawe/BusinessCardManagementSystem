using HireMind.Domain.Entities.HireMind;

namespace HireMind.Application.Commands.ManageJobs;
public record CreateJobCommand(CreateJobRequestDto Request) : IRequest<CreateJobResult>;
public record CreateJobResult(int Id);

public class CreateJobValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobValidator()
    { }
}

public class CreateJobHandler : IRequestHandler<CreateJobCommand, CreateJobResult>
{
    private readonly IJobRepository _repository;
    private readonly IHiringStageRepository _hiringStageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateJobHandler(IJobRepository jobRepository, IHiringStageRepository hiringStageRepository, IUnitOfWork unitOfWork)
    {
        _repository = jobRepository;
        _hiringStageRepository = hiringStageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateJobResult> Handle(CreateJobCommand command, CancellationToken cancellationToken)
    {
        // Create job entity
        var job = Job.Create(
            title: command.Request.Title,
            description: command.Request.Description,
            locationId: command.Request.LocationId,
            workPlaceId: command.Request.WorkPlaceId,
            contractTypeId: command.Request.ContractTypeId,
            organizationTypeId: command.Request.OrganizationTypeId,
            industrySectorId: command.Request.IndustrySectorId,
            jobTypeId: command.Request.JobTypeId,
            companyId: command.Request.CompanyId,
            startDate: command.Request.StartDate,
            endDate: command.Request.EndDate,
            isActive: command.Request.IsActive,
            questions: command.Request.Questions?.ConvertAll(q => new JobQuestion
            {
                QuestionText = q.QuestionText,
                QuestionTypeId = q.QuestionTypeId,
                IsRequired = q.IsRequired,
                AvailableAnswers = q.AvailableAnswers?.Select(a => new AnswerOption
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsPreferredAnswer = a.IsPreferredAnswer
                }).ToList() ?? new List<AnswerOption>(),
                Score = q.Score
            })
        );

        // ➤ Handle Hiring Stages
        var userStages = (command.Request.HiringStages ?? new List<CreateHiringStageDto>())
            .Where(s => !string.Equals(s.Name, "Initiate Application", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var userInputEmailTemplate = command.Request.HiringStages?
            .FirstOrDefault()?.EmailTemplate ?? string.Empty;

        // Add default first stage in backend
        var defaultFirstStage = HiringStage.Create(
            jobid: 0,
            name: "Initiate Application",
            stageOrder: 1,
            isFinalStage: false,
            isActive: false,
            viaId: 1,
            emailTemplate: !string.IsNullOrWhiteSpace(userInputEmailTemplate) ? userInputEmailTemplate
        : @"
<p>Dear Candidate,</p>
<p>Your application has been received.</p>
<p>Will come back to you soon.</p>
<p>Regards,<br/><strong>HireMind</strong></p>"
        );

        defaultFirstStage.Job = job;
        job.HiringStages.Add(defaultFirstStage); // ✅ works with read-only setter

        // Add remaining stages from frontend
        int order = 2;
        foreach (var s in userStages)
        {
            var stage = HiringStage.Create(
                jobid: 0,
                name: s.Name,
                stageOrder: s.StageOrder == 0 ? order++ : s.StageOrder,
                isFinalStage: false,
                isActive: false,
                viaId: s.ViaId,
                emailTemplate: s.EmailTemplate
            );
            stage.Job = job;

            // Map Interview Questions
            stage.InterviewQuestions = s.InterviewQuestions?.Select(q => new JobQuestion
            {
                QuestionText = q.QuestionText,
                QuestionTypeId = q.QuestionTypeId,
                IsRequired = q.IsRequired,
                AvailableAnswers = q.AvailableAnswers?.Select(a => new AnswerOption
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsPreferredAnswer = a.IsPreferredAnswer
                }).ToList() ?? new List<AnswerOption>(),
                Score = q.Score
            }).ToList() ?? new List<JobQuestion>();

            // Map Exam Questions
            stage.ExamQuestions = s.ExamQuestions?.Select(q => new JobQuestion
            {
                QuestionText = q.QuestionText,
                QuestionTypeId = q.QuestionTypeId,
                IsRequired = q.IsRequired,
                AvailableAnswers = q.AvailableAnswers?.Select(a => new AnswerOption
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsPreferredAnswer = a.IsPreferredAnswer
                }).ToList() ?? new List<AnswerOption>(),
                Score = q.Score
            }).ToList() ?? new List<JobQuestion>();

            job.HiringStages.Add(stage);
        }

        // Add job with stages in one call
        await _repository.AddAsync(job, cancellationToken);

        // Commit all changes
        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new CreateJobResult(job.Id);
    }
}
