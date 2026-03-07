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
    private readonly IUnitOfWork _unitOfWork;

    public UpdateJobHandler(IJobRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
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
                    Text = a.Text
                })
                .ToList() ?? new List<AnswerOption>(),
                PreferredAnswerId = q.PreferredAnswerId,
                Score = q.Score
            })
        );


        var result = await _repository.UpdateAsync(job, false, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new UpdateJobResult(result);
    }
}

