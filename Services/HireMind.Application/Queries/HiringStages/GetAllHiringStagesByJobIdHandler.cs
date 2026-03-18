namespace HireMind.Application.Queries.HiringStages;
public record GetAllHiringStagesByJobIdQuery(int JobId) : IRequest<GetAllHiringStagesByJobIdResult>;
public record GetAllHiringStagesByJobIdResult(List<HiringStageDto> response);

internal class GetAllHiringStagesByJobIdHandler: IRequestHandler<GetAllHiringStagesByJobIdQuery, GetAllHiringStagesByJobIdResult>
{
    private readonly IHiringStageRepository _HiringStageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllHiringStagesByJobIdHandler(IHiringStageRepository HiringStageRepository, IUnitOfWork unitOfWork)
    {
        _HiringStageRepository = HiringStageRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<GetAllHiringStagesByJobIdResult> Handle(GetAllHiringStagesByJobIdQuery request, CancellationToken cancellationToken)
    {
        var HiringStages = await _HiringStageRepository.GetByJobIdAsync(request.JobId, cancellationToken);

        var dtos = HiringStages.Adapt<List<HiringStageDto>>();

        return new GetAllHiringStagesByJobIdResult(dtos);
    }
}

