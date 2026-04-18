namespace HireMind.Api.Controllers.HireMindControllers;
[Authorize]
public class BusinessCardsController : ApiBaseController
{
    private readonly ISender _sender;

    public BusinessCardsController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Policy = PermissionConstants.BusinessCards.View)]
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchFiltersRqDto filters)
    {
        var result = await _sender.Send(new SearchQuery(filters));
        return Ok(result);
    }

    [Authorize(Policy = PermissionConstants.BusinessCards.View)]
    [HttpGet("getAll")] 
    public async Task<GetAllBusinessCardsResult> GetAllCards()
    {
        var result = await _sender.Send(new GetAllBusinessCardsQuery());
        return result;
    }

    [Authorize(Policy = PermissionConstants.BusinessCards.Create)]
    [HttpPost("add")] 
    public async Task<IActionResult> CreateCard([FromBody] CreateBusinessCardDto command)
    {
        var result = await _sender.Send(new CreateBusinessCardCommand(command));

        if (result.Id > 0)
            return Ok(result.Id);
        else
            return BadRequest("Failed to create business card.");
    }

    [Authorize(Policy = PermissionConstants.BusinessCards.View)]
    [HttpGet("get/{id}")] 
    public async Task<GetBusinessCardByIdResult> GetCardById(int id)
    {
        var result = await _sender.Send(new GetBusinessCardByIdQuery(id));

        return result;
    }

    [Authorize(Policy = PermissionConstants.BusinessCards.Delete)]
    [HttpPut("update")] 
    public async Task<IActionResult> UpdateCard([FromBody] UpdateBusinessCardDto command)
    {
        var result = await _sender.Send(new UpdateBusinessCardCommand(command));

        if (result.IsSuccess)
            return Ok(command.Id);
        else
            return BadRequest("Failed to update business card.");
    }

    [Authorize(Policy = PermissionConstants.BusinessCards.Delete)]
    [HttpDelete("delete/{id}")] 
    public async Task<IActionResult> DeleteCard(int id)
    {
        var result = await _sender.Send(new DeleteBusinessCardCommand(id));

        if (result.IsSuccess)
            return Ok(new { message = $"Business card {id} deleted successfully." });

        return NotFound(new { message = $"Business card {id} not found." });
    }

    [Authorize(Policy = PermissionConstants.BusinessCards.View)]
    [HttpPost("preview")] 
    public async Task<IActionResult> PreviewFile(IFormFile file)
    {
        return Ok(await _sender.Send(new PreviewBusinessCardsCommand(file)));
    }

    [Authorize(Policy = PermissionConstants.BusinessCards.Create)]
    [HttpPost("createMany")]
    public async Task<IActionResult> CreateMany([FromBody] List<CreateBusinessCardDto> cards)
    {
        return Ok(await _sender.Send(new CreateManyBusinessCardsCommand(cards))); 
    }

    [Authorize(Policy = PermissionConstants.BusinessCards.View)]
    [HttpPost("exportfile")]
    public async Task<IActionResult> ExportFile([FromBody] ExportRequestDto request)
    {
        var result = await _sender.Send(new ExportBusinessCardsQuery(request));
        return File(result.FileContent, result.ContentType, result.FileName);
    }

    [Authorize(Policy = PermissionConstants.BusinessCards.View)]
    [HttpPost("printpdf")]
    public async Task<IActionResult> GeneratePdf([FromBody] GeneratePdfCommand command)
    {
        var result = await _sender.Send(command);
        return File(result.FileBytes, "application/pdf", result.FileName);
    }
}
