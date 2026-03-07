using HireMind.Domain.Dtos.BusinessCard;

namespace HireMind.Application.Commands.BusinessCard;
public record CreateBusinessCardCommand(CreateBusinessCardDto Request) : IRequest<CreateBusinessCardResult>;
public record CreateBusinessCardResult(Guid Id);

public class CreateBusinessCardValidator : AbstractValidator<CreateBusinessCardCommand>
{
    public CreateBusinessCardValidator()
    {
        RuleFor(x => x.Request.ArabicName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Arabic name is required.")
            .Length(5, 50).WithMessage("Arabic name must be between 5 and 50 characters.");

        RuleFor(x => x.Request.EnglishName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("English name is required.")
            .Length(5, 50).WithMessage("English name must be between 5 and 50 characters.");

        RuleFor(x => x.Request.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.UtcNow).WithMessage("Date of birth cannot be in the future.");

        RuleFor(x => x.Request.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Request.Phone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{7,15}$").WithMessage("Invalid phone number format.")
            .MaximumLength(15);

        RuleFor(x => x.Request.Logo)
            .Must(logo =>
                 {
                     if (string.IsNullOrEmpty(logo))
                         return true;

                     var sizeInBytes = System.Text.Encoding.UTF8.GetByteCount(logo);
                     return sizeInBytes <= 1_000_000;
                 })
            .WithMessage("Photo size must not exceed 1MB.");

        RuleFor(x => x.Request.Address)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Address is required.")
            .Length(10, 200).WithMessage("Address must be between 10 and 200 characters.");
    }

}
public class CreateBusinessCardHandler : IRequestHandler<CreateBusinessCardCommand, CreateBusinessCardResult>
{
    private readonly IBusinessCardRepository _businessCardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBusinessCardHandler(IBusinessCardRepository businessCardRepository, IUnitOfWork unitOfWork)
    {
        _businessCardRepository = businessCardRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<CreateBusinessCardResult> Handle(CreateBusinessCardCommand command, CancellationToken cancellationToken)
    {
        var businessCard = businessCardModel.Create(
            command.Request.ArabicName,
            command.Request.EnglishName,
            command.Request.DateOfBirth,
            command.Request.Email,
            command.Request.Phone,
            command.Request.Logo,
            command.Request.Address
        );

        var businessCardId = await _businessCardRepository.AddAsync(businessCard, cancellationToken);
        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new CreateBusinessCardResult(businessCardId);
    }
}


