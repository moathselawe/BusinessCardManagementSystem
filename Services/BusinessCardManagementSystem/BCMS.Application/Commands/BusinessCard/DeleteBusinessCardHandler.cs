namespace BCMS.Application.Commands.BusinessCard;
public record DeleteBusinessCardCommand(Guid Id) : IRequest<DeleteBusinessCardResult>;
public record DeleteBusinessCardResult(bool IsSuccess);
public class DeleteBusinessCardHandlerValidator : AbstractValidator<DeleteBusinessCardCommand>
{
    public DeleteBusinessCardHandlerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
public class DeleteBusinessCardHandler : IRequestHandler<DeleteBusinessCardCommand, DeleteBusinessCardResult>
{
    private readonly IBusinessCardRepository _businessCardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBusinessCardHandler(IBusinessCardRepository businessCardRepository, IUnitOfWork unitOfWork)
    {
        _businessCardRepository = businessCardRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<DeleteBusinessCardResult> Handle(DeleteBusinessCardCommand request, CancellationToken cancellationToken)
    {
        var success = await _businessCardRepository.DeleteAsync(request.Id, cancellationToken);

        if (success)
            await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new DeleteBusinessCardResult(success);
    }
}

