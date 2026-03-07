using HireMind.Domain.Dtos.BusinessCard;

namespace HireMind.Application.Queries.BusinessCard;
public record GetBusinessCardByIdQuery(Guid Id) : IRequest<GetBusinessCardByIdResult>;
public record GetBusinessCardByIdResult(BusinessCardDto response);
public class GetBusinessCardByIdHandlerValidator : AbstractValidator<GetBusinessCardByIdQuery>
{
    public GetBusinessCardByIdHandlerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
internal class GetBusinessCardByIdHandler : IRequestHandler<GetBusinessCardByIdQuery, GetBusinessCardByIdResult>
{
    private readonly IBusinessCardRepository _businessCardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetBusinessCardByIdHandler(IBusinessCardRepository businessCardRepository, IUnitOfWork unitOfWork)
    {
        _businessCardRepository = businessCardRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<GetBusinessCardByIdResult> Handle(GetBusinessCardByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _businessCardRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            return new GetBusinessCardByIdResult(null!);

        var dto = entity.Adapt<BusinessCardDto>();
        return new GetBusinessCardByIdResult(dto);
    }
}
