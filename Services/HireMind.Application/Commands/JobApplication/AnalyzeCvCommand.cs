using HireMind.Domain.Entities.HireMind;
using HireMind.Domain.SeedWork;

namespace HireMind.Application.Commands.JobApplication;
public record AnalyzeCvCommand(AnalyzeCvRequestDto request) : IRequest<AnalyzeCvResult>;
public record AnalyzeCvResult(AnalyzeCvResponseDto response);

public class AnalyzeCvValidator : AbstractValidator<AnalyzeCvCommand>
{
    public AnalyzeCvValidator()
    {
        RuleFor(x => x.request.JobId).NotEmpty().WithMessage("JobId is required.");
        RuleFor(x => x.request.File).NotEmpty().WithMessage("File is required.");
    }
}

public class AnalyzeCvHandler : IRequestHandler<AnalyzeCvCommand, AnalyzeCvResult>
{
    private readonly IAnalyzeCvService _analyzeCvService;
    private readonly IJobApplicationRepository _jobApplicantRepository;
    private readonly IAnalyzeCvRepository analyzeCvRepository;
    private readonly IJobRepository _jobRepository;
    private readonly string _cvDB;
    private readonly IUnitOfWork _unitOfWork;

    public AnalyzeCvHandler(
        IAnalyzeCvService analyzeCvService,
        IJobApplicationRepository repository,
        IJobRepository jobRepository,
        IAnalyzeCvRepository analyzeCvRepository,
        IConfiguration configuration,
        IUnitOfWork unitOfWork)
    {
        _analyzeCvService = analyzeCvService;
        _jobApplicantRepository = repository;
        _jobRepository = jobRepository;
        this.analyzeCvRepository = analyzeCvRepository;
        _cvDB = configuration["Files:CVDB"];
        _unitOfWork = unitOfWork;
    }

    public async Task<AnalyzeCvResult> Handle(
        AnalyzeCvCommand command,
        CancellationToken cancellationToken)
    {

        // check if fully applied
        var isAlreadyApplied = await _jobApplicantRepository
            .CheckApplicationByEmailAndJobId(command.request.EmailAddress, command.request.JobId, cancellationToken);

        if (isAlreadyApplied)
            throw new Exception("You have already applied to this position.");

        (bool isUpdateProcess, string fullPath) = await SaveCV(command, cancellationToken);

        var job = await _jobRepository.GetByIdAsync(
            command.request.JobId,
            cancellationToken);

        if (job == null)
            throw new Exception("Job not found");

        var analyzedData = await _analyzeCvService.GetAnalyzedCvAsync(
            command.request.File,
            job,
            cancellationToken);

        var analyzeCvId = 0;
        AnalyzeCv existingAnalyzedCv = new AnalyzeCv();

        if (!isUpdateProcess)
        {
            var entity = AnalyzeCv.Create(
                command.request.JobId,
                fullPath,
                analyzedData.CvText,
                analyzedData.AiScore,
                JsonSerializer.Serialize(analyzedData.Fields),
                false,
                command.request.EmailAddress
            );

            analyzeCvId = await analyzeCvRepository.AddAsync(entity, cancellationToken);
        }
        else
        {
            existingAnalyzedCv = await analyzeCvRepository
               .GetAnalyzedCvAsync(command.request.EmailAddress, job.Id, cancellationToken);

            existingAnalyzedCv = AnalyzeCv.Update(
                existingAnalyzedCv.Id,
                command.request.JobId,
                fullPath,
                analyzedData.CvText,
                analyzedData.AiScore,
                JsonSerializer.Serialize(analyzedData.Fields),
                false,
                command.request.EmailAddress
            );

            await analyzeCvRepository.UpdateAsync(existingAnalyzedCv, false, cancellationToken);
        }

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        analyzedData.AiScore = 0;

        var response = new AnalyzeCvResponseDto
        {
            AnalyzeCvId = isUpdateProcess ? existingAnalyzedCv.Id : analyzeCvId,
            AnalyzedCvData = analyzedData
        };

        return new AnalyzeCvResult(response);
    }

    private async Task<(bool isUpdateProcess, string fullPath)> SaveCV(AnalyzeCvCommand command, CancellationToken cancellationToken)
    {
        var isUpdateProcess = false;

        // make email safe
        var safeEmail = command.request.EmailAddress
                            .Replace("@", "_")
                            .Replace(".", "_");

        // delete old CVs if exist in _cvDB
        var searchPattern = $"{safeEmail}_{command.request.JobId}_*.*";
        var existingFiles = Directory.GetFiles(_cvDB, searchPattern);

        if (existingFiles.Length > 0)
        {
            isUpdateProcess = true;
            foreach (var oldFile in existingFiles)
            {
                File.Delete(oldFile);
            }
        }

        // ensure CV folder exists
        if (!Directory.Exists(_cvDB))
            Directory.CreateDirectory(_cvDB);

        // save new CV
        var file = command.request.File;
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{safeEmail}_{command.request.JobId}_{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(_cvDB, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return (isUpdateProcess, fullPath);
    }
}

