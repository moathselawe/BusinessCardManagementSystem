using BCMS.Domain.Dtos.AI;

namespace BCMS.Application.Commands.Chatbot;
public record AISuggestionsCommand(AISuggestionsRequestDto Request) : IRequest<AISuggestionsResult>;
public record AISuggestionsResult(AISuggestionsResponseDto response);

public class AISuggestionsValidator : AbstractValidator<AISuggestionsCommand>
{
    public AISuggestionsValidator()
    {
        RuleFor(x => x.Request.SuggestionType)
            .NotEmpty().WithMessage("SuggestionType is required.");
    }
}

public class AISuggestionsHandler : IRequestHandler<AISuggestionsCommand, AISuggestionsResult>
{
    private readonly IAIService _service;

    public AISuggestionsHandler(IAIService service)
    {
        _service = service;
    }

    public async Task<AISuggestionsResult> Handle(AISuggestionsCommand command, CancellationToken cancellationToken)
    {
        var response = await _service.GetAISuggestionsAsync(command.Request, cancellationToken);

        return new AISuggestionsResult(response);
    }
}


