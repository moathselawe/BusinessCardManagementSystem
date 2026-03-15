namespace HireMind.Application.Queries.Shared;
public record GetAllLookupByParentsQuery() : IRequest<GetAllLookupParentsResult>;
public record GetAllLookupParentsResult(List<GetLookupDto> Response);

internal class GetAllLookupByParentsHandler : IRequestHandler<GetAllLookupByParentsQuery, GetAllLookupParentsResult>
{
    private readonly ILookupRepository _lookupRepository;

    public GetAllLookupByParentsHandler(ILookupRepository lookupRepository)
    {
        _lookupRepository = lookupRepository;
    }

    public async Task<GetAllLookupParentsResult> Handle(GetAllLookupByParentsQuery request, CancellationToken cancellationToken)
    {
        var dtos = await _lookupRepository.GetAllLookupParentsAsync(cancellationToken);
        return new GetAllLookupParentsResult(dtos);
    }
}

