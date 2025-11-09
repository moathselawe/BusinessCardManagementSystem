namespace BCMS.Api.Controllers;

public class BusinessCardsController : ApiBaseController
{
    private readonly ISender _sender;

    public BusinessCardsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchFiltersRqDto filters)
    {
        var result = await _sender.Send(new SearchQuery(filters));
        return Ok(result);
    }

    [HttpGet("getAll")] 
    public async Task<GetAllBusinessCardsResult> GetAllCards()
    {
        var result = await _sender.Send(new GetAllBusinessCardsQuery());
        return result;
    }

    [HttpPost("add")] 
    public async Task<IActionResult> CreateCard([FromBody] CreateBusinessCardDto command)
    {
        var result = await _sender.Send(new CreateBusinessCardCommand(command));

        if (result.Id != Guid.Empty)
            return Ok(result.Id);
        else
            return BadRequest("Failed to create business card.");
    }

    [HttpGet("get/{id}")] 
    public async Task<GetBusinessCardByIdResult> GetCardById(Guid id)
    {
        var result = await _sender.Send(new GetBusinessCardByIdQuery(id));

        return result;
    }

    [HttpPut("update")] 
    public async Task<IActionResult> UpdateCard([FromBody] UpdateBusinessCardDto command)
    {
        var result = await _sender.Send(new UpdateBusinessCardCommand(command));

        if (result.IsSuccess)
            return Ok(command.Id);
        else
            return BadRequest("Failed to update business card.");
    }

    [HttpDelete("delete/{id}")] 
    public async Task<IActionResult> DeleteCard(Guid id)
    {
        var result = await _sender.Send(new DeleteBusinessCardCommand(id));

        if (result.IsSuccess)
            return Ok(new { message = $"Business card {id} deleted successfully." });

        return NotFound(new { message = $"Business card {id} not found." });
    }

    [HttpPost("preview")] 
    public async Task<IActionResult> PreviewFile(IFormFile file)
    {
        return Ok(await _sender.Send(new PreviewBusinessCardsCommand(file)));
    }

    [HttpPost("createMany")]
    public async Task<IActionResult> CreateMany([FromBody] List<CreateBusinessCardDto> cards)
    {
        return Ok(await _sender.Send(new CreateManyBusinessCardsCommand(cards))); 
    }

    [HttpPost("exportfile")]
    public async Task<IActionResult> ExportFile([FromBody] ExportRequestDto request)
    {
        var result = await _sender.Send(new ExportBusinessCardsQuery(request));
        return File(result.FileContent, result.ContentType, result.FileName);
    }

    [HttpPost("printpdf")]
    public async Task<IActionResult> GeneratePdf([FromBody] GeneratePdfCommand command)
    {
        var result = await _sender.Send(command);
        return File(result.FileBytes, "application/pdf", result.FileName);
    }
}
