using HireMind.Application.Commands.Chatbot;
using HireMind.Domain.Dtos.AI;
using Microsoft.AspNetCore.Authorization;

namespace HireMind.Api.Controllers.AIControllers;
[Authorize]
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
