namespace HireMind.Api.Controllers.HireMindControllers;

[Authorize]
public class ManageJobsController : ApiBaseController
{
    private readonly ISender _sender;

    public ManageJobsController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Policy = PermissionConstants.Jobs.Create)]
    [HttpPost("create")]
    public async Task<CreateJobResult> Create([FromBody] CreateJobRequestDto request)
    {
        return await _sender.Send(new CreateJobCommand(request));
    }

    [Authorize(Policy = PermissionConstants.Jobs.View)]
    [HttpGet("get/{id}")]
    public async Task<GetJobByIdResult> GetJobById(int id)
    {
        var result = await _sender.Send(new GetJobByIdQuery(id));

        return result;
    }

    [Authorize(Policy = PermissionConstants.Jobs.Update)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateJob([FromBody] UpdateJobRequestDto command)
    {
        var result = await _sender.Send(new UpdateJobCommand(command));

        if (result.IsSuccess)
            return Ok(command.Id);
        else
            return BadRequest("Failed to update Job.");
    }

    [Authorize(Policy = PermissionConstants.Jobs.Update)]
    [HttpPut("updateJobActivation")]
    public async Task<IActionResult> updateJobActivation([FromBody] UpdateJobActivationRequestDto command)
    {
        var result = await _sender.Send(new UpdateJobActivationCommand(command));

        if (result.IsSuccess)
            return Ok(command.Id);
        else
            return BadRequest("Failed to update Job.");
    }

    [Authorize(Policy = PermissionConstants.Jobs.View)]
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchFiltersRqDto filters)
    {
        var result = await _sender.Send(new SearchJobsQuery(filters));
        return Ok(result);
    }

    [Authorize(Policy = PermissionConstants.Jobs.Delete)]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _sender.Send(new DeleteJobCommand(id));

        if (result.IsSuccess)
            return Ok(new { message = $"Job {id} deleted successfully." });

        return NotFound(new { message = $"Job {id} not found." });
    }
}
