using HireMind.Domain.Entities.HireMind;

namespace HireMind.Application.Commands.ManageJobs;

public record UpdateJobCommand(UpdateJobRequestDto request) : IRequest<UpdateJobResult>;
public record UpdateJobResult(bool IsSuccess);

public class UpdateJobValidator : AbstractValidator<UpdateJobCommand>
{
    public UpdateJobValidator()
    {
        // Validation rules can be added here later
    }
}

public class UpdateJobHandler : IRequestHandler<UpdateJobCommand, UpdateJobResult>
{
    private readonly IJobRepository _repository;
    private readonly IJobApplicationRepository _jobApplicationEepository;
    private readonly IHiringStageRepository _hiringStageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateJobHandler(
        IJobRepository repository,
        IJobApplicationRepository jobApplicationEepository,
        IHiringStageRepository hiringStageRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _jobApplicationEepository = jobApplicationEepository;
        _hiringStageRepository = hiringStageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateJobResult> Handle(UpdateJobCommand command, CancellationToken cancellationToken)
    {
        var job = new Job();

        // Get all job applications related to this job
        var existingApplications = await _jobApplicationEepository.GetAllJobApplicationsByJobIdAsync(command.request.Id, cancellationToken);

        // Check if job already has applications
        var isThereAnyJobApplicationsLinkedWithThisJob = existingApplications.Any();

        // If there are NO job applications → questions can be modified
        if (!isThereAnyJobApplicationsLinkedWithThisJob)
        {
            job = Job.Update(
                id: command.request.Id,
                title: command.request.Title,
                description: command.request.Description,
                locationId: command.request.LocationId,
                workPlaceId: command.request.WorkPlaceId,
                contractTypeId: command.request.ContractTypeId,
                organizationTypeId: command.request.OrganizationTypeId,
                industrySectorId: command.request.IndustrySectorId,
                jobTypeId: command.request.JobTypeId,
                companyId: command.request.CompanyId,
                startDate: command.request.StartDate,
                endDate: command.request.EndDate,
                isActive: command.request.IsActive,

                // Map questions from request
                questions: command.request.Questions?.ConvertAll(q => new JobQuestion
                {
                    QuestionText = q.QuestionText,
                    QuestionTypeId = q.QuestionTypeId,
                    IsRequired = q.IsRequired,
                    AvailableAnswers = q.AvailableAnswers?
                        .Select(a => new AnswerOption
                        {
                            Id = a.Id,
                            Text = a.Text,
                            IsPreferredAnswer = a.IsPreferredAnswer
                        }).ToList() ?? new List<AnswerOption>(),
                    Score = q.Score
                })
            );
        }
        else
        {
            // If applications exist → questions cannot be modified
            var existingJob = await _repository.GetByIdAsync(command.request.Id, cancellationToken);

            job = Job.Update(
                id: command.request.Id,
                title: command.request.Title,
                description: command.request.Description,
                locationId: command.request.LocationId,
                workPlaceId: command.request.WorkPlaceId,
                contractTypeId: command.request.ContractTypeId,
                organizationTypeId: command.request.OrganizationTypeId,
                industrySectorId: command.request.IndustrySectorId,
                jobTypeId: command.request.JobTypeId,
                companyId: command.request.CompanyId,
                startDate: command.request.StartDate,
                endDate: command.request.EndDate,
                isActive: command.request.IsActive,

                // Keep existing questions
                questions: existingJob.Questions
            );
        }

        // Update job
        await _repository.UpdateAsync(job, false, cancellationToken);


        // Get existing stages
        var existingStages = await _hiringStageRepository.GetByJobIdAsync(job.Id, cancellationToken);

        // Extract request stage ids
        var requestStageIds = command.request.HiringStages?.Where(s => s.Id != 0).Select(s => s.Id).ToList() ?? new List<int>();


        // DELETE removed stages (except first stage)
        var stagesToDelete = existingStages.Where(s => s.StageOrder != 1 && !s.IsActive && !requestStageIds.Contains(s.Id)).ToList();

        if (stagesToDelete.Any())
            await _hiringStageRepository.DeleteRangeAsync(stagesToDelete, cancellationToken);


        // Process request stages
        foreach (var stageDto in command.request.HiringStages ?? new List<UpdateHiringStageDto>())
        {
            if (stageDto.Id == 0)
                continue;

            var existingStage = existingStages.FirstOrDefault(s => s.Id == stageDto.Id);

            if (existingStage == null)
                continue;

            // First stage cannot be modified
            if (existingStage.StageOrder == 1)
                continue;

            if (existingStage.IsActive)
            {
                existingStage.UpdateDetails(
                    name: stageDto.Name,
                    stageOrder: existingStage.StageOrder,
                    isActive: existingStage.IsActive,
                    isFinalStage: stageDto.IsFinalStage,
                    viaId: existingStage.ViaId,
                    emailTemplate: stageDto.EmailTemplate
                );
            }
            else
            {
                existingStage.UpdateDetails(
                    name: stageDto.Name,
                    stageOrder: stageDto.StageOrder,
                    isActive: stageDto.IsActive,
                    isFinalStage: stageDto.IsFinalStage,
                    viaId: stageDto.ViaId,
                    emailTemplate: stageDto.EmailTemplate
                );

                existingStage.InterviewQuestions = stageDto.InterviewQuestions?.Select(q => new JobQuestion
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

                existingStage.ExamQuestions = stageDto.ExamQuestions?.Select(q => new JobQuestion
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
            }
        }
        // Save all changes
        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new UpdateJobResult(true);
    }
}