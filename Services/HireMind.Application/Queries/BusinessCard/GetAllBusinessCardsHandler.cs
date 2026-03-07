using HireMind.Domain.Dtos.BusinessCard;

namespace HireMind.Application.Queries.BusinessCard;
public record GetAllBusinessCardsQuery() : IRequest<GetAllBusinessCardsResult>;
public record GetAllBusinessCardsResult(List<BusinessCardDto> response);

internal class GetAllBusinessCardsHandler: IRequestHandler<GetAllBusinessCardsQuery, GetAllBusinessCardsResult>
{
    private readonly IBusinessCardRepository _businessCardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllBusinessCardsHandler(IBusinessCardRepository businessCardRepository, IUnitOfWork unitOfWork)
    {
        _businessCardRepository = businessCardRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<GetAllBusinessCardsResult> Handle(GetAllBusinessCardsQuery request, CancellationToken cancellationToken)
    {
        var businessCards = await _businessCardRepository.GetAllAsync(cancellationToken);

        var dtos = businessCards.Adapt<List<BusinessCardDto>>();

        return new GetAllBusinessCardsResult(dtos);
    }
}

