namespace HireMind.Application.Queries.Content;
public record GetAllAboutUsQuery() : IRequest<GetAllAboutUsResult>;
public record GetAllAboutUsResult(List<GetAboutUsDto> response);

internal class GetAllAboutUsHandler: IRequestHandler<GetAllAboutUsQuery, GetAllAboutUsResult>
{
    private readonly IAboutUsRepository _aboutUsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAboutUsHandler(IAboutUsRepository aboutUsRepository, IUnitOfWork unitOfWork)
    {
        _aboutUsRepository = aboutUsRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<GetAllAboutUsResult> Handle(GetAllAboutUsQuery request, CancellationToken cancellationToken)
    {
        var query = await _aboutUsRepository.GetAllAsync(true, cancellationToken);

        var dtos = query.Adapt<List<GetAboutUsDto>>();

        return new GetAllAboutUsResult(dtos);
    }
}

