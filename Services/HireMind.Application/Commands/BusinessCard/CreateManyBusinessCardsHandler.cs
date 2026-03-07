using HireMind.Domain.Dtos.BusinessCard;

namespace HireMind.Application.Commands.BusinessCard;

public record CreateManyBusinessCardsCommand(List<CreateBusinessCardDto> Requests) : IRequest<CreateManyBusinessCardsResult>;
public record CreateManyBusinessCardsResult(int Count); 

public class CreateManyBusinessCardsValidator : AbstractValidator<CreateManyBusinessCardsCommand>
{
    public CreateManyBusinessCardsValidator()
    {
        RuleFor(x => x.Requests).NotEmpty().WithMessage("No business cards to add.");

        RuleForEach(x => x.Requests).SetValidator(new CreateBusinessCardDtoValidator());
    }
}

public class CreateBusinessCardDtoValidator : AbstractValidator<CreateBusinessCardDto>
{
    public CreateBusinessCardDtoValidator()
    {
        RuleFor(x => x.ArabicName)
            .NotEmpty().WithMessage("Arabic name is required.")
            .Length(5, 50);

        RuleFor(x => x.EnglishName)
            .NotEmpty().WithMessage("English name is required.")
            .Length(5, 50);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.UtcNow);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress();

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{7,15}$");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .Length(10, 200);
    }
}

public class CreateManyBusinessCardsHandler : IRequestHandler<CreateManyBusinessCardsCommand, CreateManyBusinessCardsResult>
{
    private readonly IBusinessCardRepository _businessCardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateManyBusinessCardsHandler(IBusinessCardRepository businessCardRepository, IUnitOfWork unitOfWork)
    {
        _businessCardRepository = businessCardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateManyBusinessCardsResult> Handle(CreateManyBusinessCardsCommand command, CancellationToken cancellationToken)
    {
        var entities = command.Requests.Select(dto =>
            businessCardModel.Create(
                dto.ArabicName,
                dto.EnglishName,
                dto.DateOfBirth,
                dto.Email,
                dto.Phone,
                dto.Logo,
                dto.Address
            )
        ).ToList();

        var count = await _businessCardRepository.AddManyAsync(entities, cancellationToken);
        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new CreateManyBusinessCardsResult(count);
    }
}

