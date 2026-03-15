namespace HireMind.Application.Commands.ManageJobs;

public record UpdateJobCommand(UpdateJobRequestDto request) : IRequest<UpdateJobResult>;
public record UpdateJobResult(bool IsSuccess);

public class UpdateJobValidator : AbstractValidator<UpdateJobCommand>
{
    public UpdateJobValidator()
    {

    }

}
public class UpdateJobHandler : IRequestHandler<UpdateJobCommand, UpdateJobResult>
{
    private readonly IJobRepository _repository;
    private readonly IHiringStageRepository _hiringStageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateJobHandler(IJobRepository repository, IHiringStageRepository hiringStageRepository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _hiringStageRepository = hiringStageRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<UpdateJobResult> Handle(UpdateJobCommand command, CancellationToken cancellationToken)
    {    
    var job = jobModel.Update(
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
                })
                .ToList() ?? new List<AnswerOption>(),
                Score = q.Score
            })
        );


        var result = await _repository.UpdateAsync(job, false, cancellationToken);

        // 3. Handle hiring stages
        var existingStages = await _hiringStageRepository.GetByJobIdAsync(job.Id, cancellationToken);
        var requestStageIds = command.request.HiringStages?.Where(s => s.Id != 0).Select(s => s.Id).ToList() ?? new List<int>();

        // 3a. Delete removed stages
        var stagesToDelete = existingStages.Where(s => !requestStageIds.Contains(s.Id)).ToList();
        if (stagesToDelete.Any())
            await _hiringStageRepository.DeleteRangeAsync(stagesToDelete, cancellationToken);

        // 3b. Add new or update existing stages
        foreach (var stageDto in command.request.HiringStages ?? new List<UpdateHiringStageDto>())
        {
            if (stageDto.Id == 0)
            {
                // NEW stage
                var newStage = HiringStage.Create(
                    jobid: job.Id,
                    name: stageDto.Name,
                    stageOrder: stageDto.StageOrder,
                    isActive: false,
                    viaId: stageDto.ViaId,
                    emailTemplate: stageDto.EmailTemplate
                );

                newStage.InterviewQuestions = stageDto.InterviewQuestions?.Select(q => new JobQuestion
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

                newStage.ExamQuestions = stageDto.ExamQuestions?.Select(q => new JobQuestion
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

                // Add stage to job navigation for tracking
                job.HiringStages.Add(newStage);
            }
            else
            {
                // UPDATE existing stage
                var existingStage = existingStages.First(s => s.Id == stageDto.Id);
                existingStage.UpdateDetails(
                    name: stageDto.Name,
                    stageOrder: stageDto.StageOrder,
                    isActive: false,
                    viaId: stageDto.ViaId,
                    emailTemplate: stageDto.EmailTemplate
                );

                // Update questions as before
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

        // 4. Commit all changes at once
        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new UpdateJobResult(true);
    }
}

