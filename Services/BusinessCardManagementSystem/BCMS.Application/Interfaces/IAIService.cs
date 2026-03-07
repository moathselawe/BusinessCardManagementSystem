namespace BCMS.Application.Interfaces;
public interface IAIService
{
    Task<ChatbotResponseDto> GetChatbotReplyAsync(ChatbotRequestDto Request, CancellationToken cancellationToken);
    Task<AISuggestionsResponseDto> GetAISuggestionsAsync(AISuggestionsRequestDto Request, CancellationToken cancellationToken);
}

