namespace BCMS.Application.Commands.BusinessCard;
public record PreviewBusinessCardsCommand(IFormFile file) : IRequest<PreviewBusinessCardsResult>;
public record PreviewBusinessCardsResult(List<BusinessCardPreviewDto> Cards);
public class PreviewBusinessCardsValidator: AbstractValidator<PreviewBusinessCardsCommand>
{
    public PreviewBusinessCardsValidator()
    {
        RuleFor(x => x.file).NotNull().WithMessage("No file uploaded.");
    }
}
public class PreviewBusinessCardsHandler : IRequestHandler<PreviewBusinessCardsCommand, PreviewBusinessCardsResult>
{
    private readonly IFileParserService _fileParserService;

    public PreviewBusinessCardsHandler(IFileParserService fileParserService)
    {
        _fileParserService = fileParserService;
    }

    public async Task<PreviewBusinessCardsResult> Handle(PreviewBusinessCardsCommand request, CancellationToken cancellationToken)
    {
        var cards = await _fileParserService.ParseFileAsync(request.file);
        return new PreviewBusinessCardsResult(cards);
    }
}
