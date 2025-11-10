namespace BCMS.Application.Commands.BusinessCard;

public record UpdateBusinessCardCommand(UpdateBusinessCardDto Request) : IRequest<UpdateBusinessCardResult>;
public record UpdateBusinessCardResult(bool IsSuccess);

public class UpdateBusinessCardValidator : AbstractValidator<UpdateBusinessCardCommand>
{
    public UpdateBusinessCardValidator()
    {
        RuleFor(x => x.Request.Id).NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Request.ArabicName)
            .NotEmpty().WithMessage("Id is required.");

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

        RuleFor(x => x.Request.Address)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Address is required.")
            .Length(10, 200).WithMessage("Address must be between 10 and 200 characters.");
    }

}
public class UpdateBusinessCardHandler : IRequestHandler<UpdateBusinessCardCommand, UpdateBusinessCardResult>
{
    private readonly IBusinessCardRepository _businessCardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBusinessCardHandler(IBusinessCardRepository businessCardRepository, IUnitOfWork unitOfWork)
    {
        _businessCardRepository = businessCardRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<UpdateBusinessCardResult> Handle(UpdateBusinessCardCommand command, CancellationToken cancellationToken)
    {
        var businessCard = businessCardModel.Update(
            command.Request.Id,
            command.Request.ArabicName,
            command.Request.EnglishName,
            command.Request.DateOfBirth,
            command.Request.Email,
            command.Request.Phone,
            command.Request.Logo,
            command.Request.Address
        );


        var result = await _businessCardRepository.UpdateAsync(businessCard, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new UpdateBusinessCardResult(result);
    }
}

