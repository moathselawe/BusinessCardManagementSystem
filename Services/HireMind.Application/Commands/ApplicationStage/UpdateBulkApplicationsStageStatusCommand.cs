using HireMind.Domain.Dtos.UpdateApplicationStageStatusRequestDto;
using HireMind.Domain.Enum;

namespace HireMind.Application.Commands.ApplicationStage;

public record UpdateBulkApplicationsStageStatusCommand(UpdateBulkApplicationsStageStatusRequestDto Request) : IRequest<UpdateBulkApplicationsStageStatusResult>;
public record UpdateBulkApplicationsStageStatusResult(bool IsSuccess);

public class UpdateBulkApplicationsStageStatusValidator : AbstractValidator<UpdateBulkApplicationsStageStatusCommand>
{
    public UpdateBulkApplicationsStageStatusValidator()
    {

    }

}
public class UpdateBulkApplicationsStageStatusHandler : IRequestHandler<UpdateBulkApplicationsStageStatusCommand, UpdateBulkApplicationsStageStatusResult>
{
    private readonly IApplicationStageRepository _applicationStageRepository;
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly IHiringStageRepository _hiringStageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBulkApplicationsStageStatusHandler(IApplicationStageRepository repository,
        IJobApplicationRepository jobApplicationRepository,

        IHiringStageRepository hiringStageRepository,
        IUnitOfWork unitOfWork)
    {
        _applicationStageRepository = repository;
        _jobApplicationRepository = jobApplicationRepository;
        _hiringStageRepository = hiringStageRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<UpdateBulkApplicationsStageStatusResult> Handle(UpdateBulkApplicationsStageStatusCommand command, CancellationToken cancellationToken)
    {

        switch (command.Request.NewStatus)
        {
            case StageStatus.New:
            case StageStatus.Selected:
            case StageStatus.NotSelected:
                await _applicationStageRepository.UpdateBulkApplicationStagesStatusBulkAsync(command.Request.Ids, command.Request.NewStatus, cancellationToken);
                break;

            case StageStatus.NextStage:
                var alreadyInLastStage = new List<int>();

                // جلب المراحل وترتيبها
                var hiringStages = await _hiringStageRepository.GetByJobIdAsync(command.Request.JobId, cancellationToken);

                var orderedStages = hiringStages.OrderBy(x => x.StageOrder).ToList();

                // جلب الطلبات دفعة واحدة
                var jobApplications = await _jobApplicationRepository
                    .GetByIdsWithCurrentStageAsync(command.Request.Ids, cancellationToken);

                // 🔥 هنا نخزن كل الـ stages الجديدة
                var newStages = new List<applicationStage>();

                foreach (var jobApplication in jobApplications)
                {
                    if (jobApplication.CurrentStage == null)
                    {
                        alreadyInLastStage.Add(jobApplication.Id);
                        continue;
                    }

                    var currentStageId = jobApplication.CurrentStage.HiringStageId;

                    var currentStage = orderedStages
                        .FirstOrDefault(x => x.Id == currentStageId);

                    if (currentStage == null)
                    {
                        alreadyInLastStage.Add(jobApplication.Id);
                        continue;
                    }

                    var nextStage = orderedStages
                        .FirstOrDefault(x => x.StageOrder > currentStage.StageOrder);

                    if (nextStage == null)
                    {
                        alreadyInLastStage.Add(jobApplication.Id);
                        continue;
                    }

                    // تحديث المرحلة الحالية
                    jobApplication.CurrentStage.Status = StageStatus.Selected;

                    // تفعيل المرحلة التالية إذا لازم
                    if (!nextStage.IsActive)
                    {
                        nextStage.UpdateDetails(
                            nextStage.Name,
                            nextStage.StageOrder,
                            true,
                            false,
                            nextStage.ViaId,
                            nextStage.EmailTemplate
                        );
                    }

                    // إنشاء المرحلة الجديدة
                    var newStage = applicationStage.Create(
                        jobApplication.Id,
                        nextStage.Id,
                        StageStatus.New
                    );

                    // نخزنها بدل ما نحفظ مباشرة
                    newStages.Add(newStage);


                    jobApplication.SetCurrentStage(newStage);
                }

                // 🔥 أهم خطوة: إدخال الكل مرة واحدة
                if (newStages.Any())
                {
                    await _applicationStageRepository.AddRangeAsync(newStages, cancellationToken);
                }

                break;

            case StageStatus.Approved:

                // جلب المراحل
                 hiringStages = await _hiringStageRepository
                   .GetByJobIdAsync(command.Request.JobId, cancellationToken);

                 orderedStages = hiringStages
                   .OrderBy(x => x.StageOrder)
                   .ToList();

                var lastStage = orderedStages.LastOrDefault();

                // جلب الطلبات
                 jobApplications = await _jobApplicationRepository
                   .GetByIdsWithCurrentStageAsync(command.Request.Ids, cancellationToken);

                foreach (var jobApplication in jobApplications)
                {
                    if (jobApplication.CurrentStage == null)
                        continue;

                    var currentHiringStageId = jobApplication.CurrentStage.HiringStageId;

                    // تحقق إذا كانت هذه المرحلة هي FinalStage
                    bool isFinalStage = jobApplication.CurrentStage.HiringStage.IsFinalStage;

                    // تحقق إذا كانت هذه آخر مرحلة
                    bool isLastStage = lastStage != null && lastStage.Id == currentHiringStageId;

                    // يسمح بالـ approve فقط إذا كانت FinalStage أو LastStage
                    if (isFinalStage || isLastStage)
                    {
                        // تحديث حالة المرحلة
                        jobApplication.CurrentStage.Status = StageStatus.Approved;

                        // إذا لم تكن FinalStage، نجعلها FinalStage
                        if (!isFinalStage)
                        {
                            jobApplication.CurrentStage.HiringStage.MarkAsFinalStage();
                        }
                    }
                }

                break;
            default:
                throw new Exception("Unsupported status operation");
        }

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new UpdateBulkApplicationsStageStatusResult(true);
    }
}



