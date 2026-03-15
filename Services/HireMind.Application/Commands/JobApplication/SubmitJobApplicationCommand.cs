using DocumentFormat.OpenXml.ExtendedProperties;
using GenerativeAI.Types;
using HireMind.Domain.Entities;
using HireMind.Domain.Enum;
using HireMind.Domain.IRepositories;
using System.Runtime.ConstrainedExecution;

namespace HireMind.Application.Commands.JobApplication;
public record SubmitJobApplicationCommand(SubmitJobApplicationRequestDto request) : IRequest<SubmitJobApplicationResult>;
public record SubmitJobApplicationResult(int Id);

public class SubmitJobApplicationValidator : AbstractValidator<SubmitJobApplicationCommand>
{
    public SubmitJobApplicationValidator()
    {
        RuleFor(x => x.request.JobId).NotEmpty().WithMessage("JobId is required.");
        RuleFor(x => x.request.AnalyzeCvId).NotEmpty().WithMessage("File is required.");
        RuleFor(x => x.request.Answers).NotEmpty().WithMessage("Answers is required.");
    }
}

public class SubmitJobApplicationHandler : IRequestHandler<SubmitJobApplicationCommand, SubmitJobApplicationResult>
{
    private readonly IJobApplicationRepository _repository;
    private readonly IJobRepository _jobRepository;
    private readonly IAnalyzeCvRepository _analyzeCvRepository;
    private readonly IApplicationStageRepository _applicationStageRepository;
    private readonly IUnitOfWork _unitOfWork;


    public SubmitJobApplicationHandler(
        IJobApplicationRepository repository,
        IJobRepository jobRepository,
        IAnalyzeCvRepository analyzeCvRepository,
        IApplicationStageRepository applicationStageRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _jobRepository = jobRepository;
        _analyzeCvRepository = analyzeCvRepository;
        _applicationStageRepository = applicationStageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SubmitJobApplicationResult> Handle(
    SubmitJobApplicationCommand command,
    CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(
            command.request.JobId,
            cancellationToken);

        if (job == null)
            throw new Exception("Job not found");

        // get score of the user answers
        var systemScore = CalculateScore(command.request.Answers, job);

        // get AI analyzed CV
        var analyzedCVByAI = await _analyzeCvRepository.GetByIdAsync(
            (int)command.request.AnalyzeCvId,
            cancellationToken);

        if (analyzedCVByAI == null)
            throw new Exception("Analyze CV not found");

        // calculate total score
        var totalScore = (analyzedCVByAI.AiScore * 0.4) + (systemScore * 0.6);

        // activate analyzed CV
        analyzedCVByAI.Activate();

        // create job application
        var entity = jobApplication.Create(
            command.request.JobId,
            (int)command.request.AnalyzeCvId,
            systemScore,
            totalScore,
            JsonSerializer.Serialize(command.request.Answers),
            PersonalInfo.Create(
              fullName: command.request.PersonalInfo.FullName,
              mobileNumber: command.request.PersonalInfo.MobileNumber,
              emailAddress: command.request.PersonalInfo.EmailAddress,
              countryCodeId: command.request.PersonalInfo.CountryCodeId
            )
        );

        var submitJobApplicationId = await _repository.AddAsync(entity, cancellationToken);

        var hiringStage = job.HiringStages.FirstOrDefault(s => s.JobId == job.Id && s.StageOrder == 1);
        if (hiringStage?.Id == null)
            throw new Exception("Intial stage not found");

        var applicationStage = ApplicationStage.Create(
            jobApplicationId: submitJobApplicationId,
            hiringStageId: hiringStage!.Id,
            status: StageStatus.New
         );

        var submitApplicationStageId = await _applicationStageRepository.AddAsync(applicationStage, cancellationToken);

        entity.CurrentStageId = submitApplicationStageId;

        await _repository.UpdateAsync(entity, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new SubmitJobApplicationResult(submitJobApplicationId);
    }

    private double CalculateScore(Dictionary<string, object?> answers, Job job)
    {
        double totalScore = 0;
        double maxScore = 0;

        for (int i = 0; i < job.Questions.Count; i++)
        {
            var question = job.Questions[i];
            var key = $"Q_{i}";

            maxScore += question.Score;

            if (!answers.TryGetValue(key, out var answer) || answer == null)
                continue;

            var preferredAnswers = question.AvailableAnswers
                .Where(a => a.IsPreferredAnswer)
                .Select(a => a.Id)
                .ToList();

            if (!preferredAnswers.Any())
                continue;

            // JsonElement handling
            if (answer is JsonElement element)
            {
                // Single answer (radio / dropdown)
                if (element.ValueKind == JsonValueKind.String)
                {
                    var value = element.GetString();

                    if (preferredAnswers.Contains(value))
                    {
                        totalScore += question.Score;
                    }
                }

                // Multiple choice
                if (element.ValueKind == JsonValueKind.Array)
                {
                    var selectedAnswers = element
                        .EnumerateArray()
                        .Select(x => x.GetString())
                        .Where(x => x != null)
                        .ToList();

                    if (selectedAnswers.Any(x => preferredAnswers.Contains(x!)))
                    {
                        totalScore += question.Score;
                    }
                }
            }
        }

        if (maxScore == 0)
            return 0;

        return Math.Round((totalScore / maxScore) * 100, 2);
    }
}
