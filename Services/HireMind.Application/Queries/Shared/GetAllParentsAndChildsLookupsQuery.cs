namespace HireMind.Application.Queries.Shared;
public record GetAllParentsAndChildsLookupsQuery() : IRequest<GetAllParentsAndChildsLookupsResult>;
public record GetAllParentsAndChildsLookupsResult(List<GetAllLookupsPartenersAndChildrensDto> Response);

internal class GetAllParentsAndChildsLookupsHandler : IRequestHandler<GetAllParentsAndChildsLookupsQuery, GetAllParentsAndChildsLookupsResult>
{
    private readonly ILookupRepository _lookupRepository;

    public GetAllParentsAndChildsLookupsHandler(ILookupRepository lookupRepository)
    {
        _lookupRepository = lookupRepository;
    }

    public async Task<GetAllParentsAndChildsLookupsResult> Handle(GetAllParentsAndChildsLookupsQuery request, CancellationToken cancellationToken)
    {
        var dtos = await _lookupRepository.GetAllParentsAndChildsLookupsAsync(cancellationToken);
        return new GetAllParentsAndChildsLookupsResult(dtos);
    }
}

