namespace HireMind.Api.Controllers.NewFolder;

[Authorize]
public class LookUpsController : ApiBaseController
{
    private readonly ISender _sender;

    public LookUpsController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Policy = PermissionConstants.Lookups.View)]
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchFiltersRqDto filters)
    {
        var result = await _sender.Send(new SearchLookupsQuery(filters));
        return Ok(result);
    }

    [Authorize(Policy = PermissionConstants.Lookups.View)]
    [HttpGet("getById/{id}")]
    public async Task<GetLookupByIdResult> GetJobById(int id)
    {
        var result = await _sender.Send(new GetLookupByIdQuery(id));

        return result;
    }

    [Authorize(Policy = PermissionConstants.Lookups.View)]
    [HttpGet("getAllByName")]
    public async Task<GetLookupByNameResult> GetAllByName(string name)
    {
        var result = await _sender.Send(new GetLookupByNameQuery(name));
        return result;
    }

    [Authorize(Policy = PermissionConstants.Lookups.View)]
    [HttpGet("getAllParents")]
    public async Task<GetAllLookupParentsResult> getAllParents()
    {
        var result = await _sender.Send(new GetAllLookupByParentsQuery());
        return result;
    }

    [Authorize(Policy = PermissionConstants.Lookups.View)]
    [HttpGet("getAllParentsAndChilds")]
    public async Task<GetAllParentsAndChildsLookupsResult> getAllParentsAndChilds()
    {
        var result = await _sender.Send(new GetAllParentsAndChildsLookupsQuery());
        return result;
    }

    [Authorize(Policy = PermissionConstants.Lookups.Create)]
    [HttpPost("createLookup")]
    public async Task<IActionResult> Create([FromBody] CreateLookUpDto command)
    {
        var result = await _sender.Send(new CreateLookupCommand(command));

        if (result.Id > 0)
            return Ok(result.Id);
        else
            return BadRequest("Failed to create lookup.");
    }

    [Authorize(Policy = PermissionConstants.Lookups.Update)]
    [HttpPut("updateLookup")]
    public async Task<IActionResult> Update([FromBody] UpdateLookUpDto command)
    {
        var result = await _sender.Send(new UpdateLookupCommand(command));

        if (result.IsSuccess)
            return Ok(command.Id);
        else
            return NotFound("Lookup not found.");
    
    }

    [Authorize(Policy = PermissionConstants.Lookups.Delete)]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _sender.Send(new DeleteLookupCommand(id));

        if (result.IsSuccess)
            return Ok(new { message = $"Job {id} deleted successfully." });

        return NotFound(new { message = $"Job {id} not found." });
    }
}


