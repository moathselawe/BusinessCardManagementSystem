using BCMS.Domain.Dtos.AI;

namespace BCMS.Application.Commands.Chatbot;
public record ChatbotCommand(ChatbotRequestDto Request) : IRequest<ChatbotResult>;
public record ChatbotResult(ChatbotResponseDto response);

public class ChatbotValidator : AbstractValidator<ChatbotCommand>
{
    public ChatbotValidator()
    {
        RuleFor(x => x.Request.Message)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(100).WithMessage("Message is too long.");
    }
}

public class ChatbotHandler : IRequestHandler<ChatbotCommand, ChatbotResult>
{
    private readonly IAIService _chatbotService;

    public ChatbotHandler(IAIService chatbotService)
    {
        _chatbotService = chatbotService;
    }

    public async Task<ChatbotResult> Handle(ChatbotCommand command, CancellationToken cancellationToken)
    {
        var response = await _chatbotService.GetChatbotReplyAsync(command.Request, cancellationToken);

        return new ChatbotResult(response);
    }
}


