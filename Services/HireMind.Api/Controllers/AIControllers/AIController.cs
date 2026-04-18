namespace HireMind.Api.Controllers.AIControllers;
[Authorize]
public class AIController : ApiBaseController
{
    private readonly ISender _sender;

    public AIController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Policy = PermissionConstants.AI.Chat)]
    [HttpPost("chatbot")]
    public async Task<ChatbotResult> Chatbot([FromBody] ChatbotRequestDto command)
    {
        return await _sender.Send(new ChatbotCommand(command));
    }

    [Authorize(Policy = PermissionConstants.AI.Suggest)]
    [HttpPost("suggestions")]
    public async Task<AISuggestionsResult> Suggestions([FromBody] AISuggestionsRequestDto command)
    {
        return await _sender.Send(new AISuggestionsCommand(command));
    }
}
