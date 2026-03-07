using BCMS.Application.Commands.Chatbot;
using BCMS.Domain.Dtos.AI;

namespace BCMS.Api.Controllers.AIControllers;

public class AIController : ApiBaseController
{
    private readonly ISender _sender;

    public AIController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("chatbot")]
    public async Task<ChatbotResult> Chatbot([FromBody] ChatbotRequestDto command)
    {
        return await _sender.Send(new ChatbotCommand(command));
    }

    [HttpPost("suggestions")]
    public async Task<AISuggestionsResult> Suggestions([FromBody] AISuggestionsRequestDto command)
    {
        return await _sender.Send(new AISuggestionsCommand(command));
    }
}
