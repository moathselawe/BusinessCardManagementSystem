using HireMind.Domain.Dtos.BusinessCard;
using HireMind.Domain.Dtos.SharedDtos;

namespace HireMind.Application.Queries.BusinessCard;

public record SearchQuery(SearchFiltersRqDto Filters) : IRequest<SearchFiltersRsDto<BusinessCardDto>>;

internal class SearchHandler : IRequestHandler<SearchQuery, SearchFiltersRsDto<BusinessCardDto>>
{
    private readonly IBusinessCardRepository _repo;

    public SearchHandler(IBusinessCardRepository repo)
    {
        _repo = repo;
    }

    public async Task<SearchFiltersRsDto<BusinessCardDto>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        return await _repo.SearchAsync(request.Filters, cancellationToken);
    }
}
