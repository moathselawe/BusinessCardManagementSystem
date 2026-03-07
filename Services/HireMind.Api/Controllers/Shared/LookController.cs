using HireMind.Application.Commands.Shared;

namespace HireMind.Api.Controllers.NewFolder;

public class LookController : ApiBaseController
{
    private readonly ISender _sender;

    public LookController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("getAllByName")]
    public async Task<GetLookupByNameResult> GetAllByName(string name)
    {
        var result = await _sender.Send(new GetLookupByNameQuery(name));
        return result;
    }

    [HttpPost("createLookup")]
    public async Task<IActionResult> Create([FromBody] CreateLookUpDto command)
    {
        var result = await _sender.Send(new CreateLookupCommand(command));

        if (result.Id != Guid.Empty)
            return Ok(result.Id);
        else
            return BadRequest("Failed to create lookup.");
    }

    [HttpPut("updateLookup")]
    public async Task<IActionResult> Update([FromBody] UpdateLookUpDto command)
    {
        var result = await _sender.Send(new UpdateLookupCommand(command));

        if (result.IsSuccess)
            return Ok("Lookup updated successfully.");
        else
            return NotFound("Lookup not found.");
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _sender.Send(new DeleteLookupCommand(id));

        if (result.IsSuccess)
            return Ok(new { message = $"Job {id} deleted successfully." });

        return NotFound(new { message = $"Job {id} not found." });
    }
}


