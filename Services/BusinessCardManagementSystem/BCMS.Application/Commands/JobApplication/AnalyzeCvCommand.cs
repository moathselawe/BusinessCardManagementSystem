namespace BCMS.Application.Commands.JobApplication;
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
    private readonly IJobRepository _jobRepository;

    public AnalyzeCvHandler(IAnalyzeCvService analyzeCvService, IJobRepository jobRepository)
    {
        _analyzeCvService = analyzeCvService;
        _jobRepository = jobRepository;
    }

    public async Task<AnalyzeCvResult> Handle(
        AnalyzeCvCommand command,
        CancellationToken cancellationToken)
    {
        // 1️⃣ Get job
        var job = await _jobRepository.GetByIdAsync(
            command.request.JobId,
            cancellationToken);

        if (job == null)
            throw new Exception("Job not found");

        // 2️⃣ Convert job questions → AI fields
        var jobFields = MapQuestionsToFields(job);

        // 3️⃣ Analyze CV
        var analyzedData = await _analyzeCvService.GetAnalyzedCvAsync(
            command.request.File,
            jobFields,
            cancellationToken);

        // 4️⃣ Return response
        var response = new AnalyzeCvResponseDto
        {
            AnalyzedCvData = analyzedData
        };

        return new AnalyzeCvResult(response);
    }
    private List<JobFieldDto> MapQuestionsToFields(Job job)
    {
        return job.Questions.Select((q, index) => new JobFieldDto
        {
            FieldName = $"Question_{index}",
            DisplayName = q.QuestionText,
            IsRequired = q.IsRequired,
            FieldType = GetFieldType(q.QuestionTypeId)
        }).ToList();
    }
    private string GetFieldType(int questionTypeId)
    {
        return questionTypeId switch
        {
            1 => "string",   // text
            2 => "number",   // numeric
            3 => "date",     // date
            4 => "list",     // dropdown
            5 => "boolean",  // yes/no
            _ => "string"
        };
    }
}



//        //// 1️ Get job fields
//        //var jobFields = await _jobRepository.GetJobFieldsAsync(command.JobId, cancellationToken);
//        // Test data instead of fetching from DB
//        var jobFields = new List<JobFieldDto>
//        {
//            new JobFieldDto { FieldName = "FullName", DisplayName = "Full Name", IsRequired = true, FieldType = "string" },
//            new JobFieldDto { FieldName = "Email", DisplayName = "Email Address", IsRequired = true, FieldType = "string" },
//            new JobFieldDto { FieldName = "Phone", DisplayName = "Phone Number", IsRequired = false, FieldType = "string" },
//            new JobFieldDto { FieldName = "Skills", DisplayName = "Skills", IsRequired = false, FieldType = "list" }
//        };

//        // 2️ Analyze CV using AI
//        var analyzedCvData = await _analyzeCvService.GetAnalyzedCvAsync(command.file, jobFields, cancellationToken);

//        // 3️ Save analyzed data in DB
//        //var analyzedCvId = await _analyzeCvRepository.AddAsync(analyzedCvData, cancellationToken);

//        //// 4️ Save CV file
//        //var fileId = await _fileRepository.AddAsync(command.file, cancellationToken);

//        //// 5️ Create draft application
//        //var draftApplicationId = await _draftJobApplicationRepository.CreateNewApplicationAsync(
//        //    jobId: command.JobId,
//        //    fileId: fileId,
//        //    analyzedCvId: analyzedCvId,
//        //    cancellationToken: cancellationToken
//        //);

//        //// 6️ Commit all changes 
//        //await _unitOfWork.SaveWorkAsync(cancellationToken);

//        // 7️ Return response to UI
//        var response = new AnalyzeCvResponseDto
//        {
//            //DraftApplicationId = draftApplicationId,
//            //FileId = fileId,
//            AnalyzedCvData = analyzedCvData
//        };

//        return new AnalyzeCvResult(response);