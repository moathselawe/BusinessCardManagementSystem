namespace HireMind.Application.Queries.Shared;
public record GetLookupByNameQuery(string CategoryName) : IRequest<GetLookupByNameResult>;
public record GetLookupByNameResult(List<GetLookupDto> Response);

internal class GetLookupByNameHandler : IRequestHandler<GetLookupByNameQuery, GetLookupByNameResult>
{
    private readonly ILookupRepository _lookupRepository;

    public GetLookupByNameHandler(ILookupRepository lookupRepository)
    {
        _lookupRepository = lookupRepository;
    }

    public async Task<GetLookupByNameResult> Handle(GetLookupByNameQuery request, CancellationToken cancellationToken)
    {
        var dtos = await _lookupRepository.GetAllByNameAsync(request.CategoryName, cancellationToken);
        return new GetLookupByNameResult(dtos);
    }
}

