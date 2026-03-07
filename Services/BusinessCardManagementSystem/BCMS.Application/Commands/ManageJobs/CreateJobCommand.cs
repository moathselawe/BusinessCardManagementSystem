namespace BCMS.Application.Commands.ManageJobs;
public record CreateJobCommand(CreateJobRequestDto request) : IRequest<CreateJobResult>;
public record CreateJobResult(Guid Id);

public class CreateJobValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobValidator()
    { }
}

public class CreateJobHandler : IRequestHandler<CreateJobCommand, CreateJobResult>
{
    private readonly IJobRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateJobHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork)
    {
        _repository = jobRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<CreateJobResult> Handle(CreateJobCommand command, CancellationToken cancellationToken)
    {
        var job = jobModel.Create(
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
                    Text = a.Text
                })
                .ToList() ?? new List<AnswerOption>(),
                PreferredAnswerId = q.PreferredAnswerId,
                Score = q.Score
            })
        );

        var jobId = await _repository.AddAsync(job, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new CreateJobResult(jobId);
    }
}
